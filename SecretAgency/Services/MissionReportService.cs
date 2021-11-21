using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SecretAgency.Constants;
using SecretAgency.Models;
using SecretAgency.Repositories.Interfaces;
using SecretAgency.Services.Interfaces;
using Serilog;

namespace SecretAgency.Services
{
    public class MissionReportService : IMissionReportService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMissionReportRepository _missionReportRepository;
        private readonly IMissionService _missionService;

        public MissionReportService(IConfiguration configuration, IMissionReportRepository missionReportRepository, IMissionService missionService)
        {
            _logger = Log.ForContext<MissionReportService>();
            _missionReportRepository = missionReportRepository;
            _missionService = missionService;

            _configuration = configuration;
        }

        public async Task<IResult<bool>> DeleteMissionReport(Guid id)
        {
            if ((await MissionReportExists(id)).HasFailedElseReturn(out var missionReportExists))
            {
                return ServiceResult.Failure<bool>(Errors.DatabaseError);
            }

            if (!missionReportExists)
            {
                return ServiceResult.Failure<bool>(Errors.MissionReportNotFound);
            }

            return (await _missionReportRepository.Delete(id)).HasFailedElseReturn(out var isDeleted)
                ? ServiceResult.Failure<bool>(Errors.DatabaseError)
                : ServiceResult.Success(isDeleted);
        }

        public async Task<IResult<MissionReport>> UpdateMissionReport(Guid id, MissionReportDto missionReportDto)
        {
            if ((await MissionReportExists(id)).HasFailedElseReturn(out var missionReportExists))
            {
                return ServiceResult.Failure<MissionReport>(Errors.DatabaseError);
            }

            if (!missionReportExists)
            {
                return ServiceResult.Failure<MissionReport>(Errors.MissionReportNotFound);
            }

            if ((await GetMissionReportById(id)).HasFailedElseReturn(out var originalMissionReport))
            {
                return ServiceResult.Failure<MissionReport>(Errors.DatabaseError);
            }

            var updatedMissionReport = ApplyMissionReportDtoToMissionReport(originalMissionReport, missionReportDto);

            return (await _missionReportRepository.Update(id, updatedMissionReport)).HasFailedElseReturn(out var result)
                ? ServiceResult.Failure<MissionReport>(Errors.DatabaseError)
                : ServiceResult.Success(result);
        }

        public async Task<IResult<MissionReport>> AddMissionReport(MissionReportDto missionReportDto)
        {
            var missionReport = BuildMissionReportFromDto(missionReportDto);

            if ((await MissionReportExists(missionReport.Id)).HasFailedElseReturn(out var missionReportExists))
            {
                return ServiceResult.Failure<MissionReport>(Errors.DatabaseError);
            }

            if (missionReportExists)
            {
                return ServiceResult.Failure<MissionReport>(Errors.MissionReportAlreadyExists);
            }

            var missionResult = await _missionService.GetMissionById(missionReport.MissionId);
            
            if (!missionResult.IsSuccessful)
            {
                return ServiceResult.Failure<MissionReport>(missionResult.Errors);
            }

            var mission = missionResult.Value;

            if (mission.HasTimeLimit && (DateTime.UtcNow > mission.ValidToUTC || DateTime.UtcNow < mission.ValidFromUTC))
            {
                return ServiceResult.Failure<MissionReport>(Errors.MissionNotAcceptingReports);
            }
            
            // TODO validate that agent ID exists here

            return (await _missionReportRepository.Create(missionReport)).HasFailedElseReturn(out var result)
                ? ServiceResult.Failure<MissionReport>(Errors.DatabaseError)
                : ServiceResult.Success(result);
        }

        public async Task<IResult<IEnumerable<MissionReport>>> GetPendingReports()
        {
            return (await _missionReportRepository.GetAllInState(MissionReportApprovalState.Pending)).HasFailedElseReturn(out var result)
                ? ServiceResult.Failure<IEnumerable<MissionReport>>(Errors.DatabaseError)
                : ServiceResult.Success(result);
        }

        public async Task<IResult<MissionReport>> GetMissionReportById(Guid id)
        {
            if ((await MissionReportExists(id)).HasFailedElseReturn(out var missionReportExists))
            {
                return ServiceResult.Failure<MissionReport>(Errors.DatabaseError);
            }

            if (!missionReportExists)
            {
                return ServiceResult.Failure<MissionReport>(Errors.MissionReportNotFound);
            }

            return (await _missionReportRepository.Get(id)).HasFailedElseReturn(out var result)
                ? ServiceResult.Failure<MissionReport>(Errors.DatabaseError)
                : ServiceResult.Success(result);
        }

        public async Task<IResult<MissionReport>> SetMissionState(Guid id, MissionReportApprovalState state)
        {
            if ((await MissionReportExists(id)).HasFailedElseReturn(out var missionReportExists))
            {
                return ServiceResult.Failure<MissionReport>(Errors.DatabaseError);
            }

            if (!missionReportExists)
            {
                return ServiceResult.Failure<MissionReport>(Errors.MissionReportNotFound);
            }

            if ((await _missionReportRepository.Get(id)).HasFailedElseReturn(out var missionReport))
            {
                return ServiceResult.Failure<MissionReport>(Errors.DatabaseError);
            }

            var updatedMissionReport = missionReport with { State = state };

            return (await _missionReportRepository.Update(id, updatedMissionReport)).HasFailedElseReturn(out var result)
                ? ServiceResult.Failure<MissionReport>(Errors.DatabaseError)
                : ServiceResult.Success(result);
        }

        public async Task<IResult<bool>> MissionReportExists(Guid id)
        {
            return (await _missionReportRepository.Exists(id)).HasFailedElseReturn(out var missionExists)
                ? ServiceResult.Failure<bool>(Errors.DatabaseError)
                : ServiceResult.Success(missionExists);
        }

        private static MissionReport BuildMissionReportFromDto(MissionReportDto original) => new MissionReport()
        {
            MissionId = original.MissionId,
            FieldNotes = original.FieldNotes,
            Password = original.Password,
            TwitterHandle = original.TwitterHandle
        };

        private static MissionReport ApplyMissionReportDtoToMissionReport(MissionReport original, MissionReportDto update) => original with
        {
            FieldNotes = update.FieldNotes,
            Password = update.Password,
            TwitterHandle = update.TwitterHandle
        };
    }
}
