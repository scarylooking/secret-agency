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
    public class MissionService : IMissionService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMissionRepository _missionRepository;

        public MissionService(IConfiguration configuration, IMissionRepository missionRepository)
        {
            _logger = Log.ForContext<MissionService>();
            _missionRepository = missionRepository;

            _configuration = configuration;
        }

        public async Task<IResult<bool>> DeleteMission(Guid id)
        {
            if ((await MissionExists(id)).HasFailedElseReturn(out var missionExists))
            {
                return ServiceResult.Failure<bool>(Errors.DatabaseError);
            }

            if (!missionExists)
            {
                return ServiceResult.Failure<bool>(Errors.MissionNotFound);
            }

            return (await _missionRepository.Delete(id)).HasFailedElseReturn(out var isDeleted)
                ? ServiceResult.Failure<bool>(Errors.DatabaseError)
                : ServiceResult.Success(isDeleted);
        }

        public async Task<IResult<Mission>> UpdateMission(Guid id, MissionDto missionDto)
        {
            if ((await MissionExists(id)).HasFailedElseReturn(out var missionExists))
            {
                return ServiceResult.Failure<Mission>(Errors.DatabaseError);
            }

            if (!missionExists)
            {
                return ServiceResult.Failure<Mission>(Errors.MissionNotFound);
            }

            if ((await GetMissionById(id)).HasFailedElseReturn(out var originalMission))
            {
                return ServiceResult.Failure<Mission>(Errors.DatabaseError);
            }

            var updatedMission = ApplyMissionDtoToMissionObject(originalMission, missionDto);

            return (await _missionRepository.Update(id, updatedMission)).HasFailedElseReturn(out var result)
                ? ServiceResult.Failure<Mission>(Errors.DatabaseError)
                : ServiceResult.Success(result);
        }

        public async Task<IResult<Mission>> AddMission(MissionDto missionDto)
        {
            var mission = BuildMissionObjectFromDto(missionDto);

            if ((await MissionExists(mission.Id)).HasFailedElseReturn(out var missionExists))
            {
                return ServiceResult.Failure<Mission>(Errors.DatabaseError);
            }

            if (missionExists)
            {
                return ServiceResult.Failure<Mission>(Errors.MissionAlreadyExists);
            }

            return (await _missionRepository.Create(mission)).HasFailedElseReturn(out var result)
                ? ServiceResult.Failure<Mission>(Errors.DatabaseError)
                : ServiceResult.Success(result);
        }

        public async Task<IResult<IEnumerable<Mission>>> GetAllMissions()
        {
            return (await _missionRepository.GetAll()).HasFailedElseReturn(out var result)
                ? ServiceResult.Failure<IEnumerable<Mission>>(Errors.DatabaseError)
                : ServiceResult.Success(result);
        }

        public async Task<IResult<Mission>> GetMissionById(Guid id)
        {
            if ((await MissionExists(id)).HasFailedElseReturn(out var missionExists))
            {
                return ServiceResult.Failure<Mission>(Errors.DatabaseError);
            }

            if (!missionExists)
            {
                return ServiceResult.Failure<Mission>(Errors.MissionNotFound);
            }

            return (await _missionRepository.Get(id)).HasFailedElseReturn(out var result)
                ? ServiceResult.Failure<Mission>(Errors.DatabaseError)
                : ServiceResult.Success(result);
        }

        public async Task<IResult<bool>> MissionExists(Guid id)
        {
            return (await _missionRepository.Exists(id)).HasFailedElseReturn(out var missionExists)
                ? ServiceResult.Failure<bool>(Errors.DatabaseError)
                : ServiceResult.Success(missionExists);
        }

        private static Mission BuildMissionObjectFromDto(MissionDto original) => new Mission()
        {
            Title = original.Title,
            Description = original.Description,
            Steps = original.Steps,
            ValidToUTC = original.ValidToUTC,
            ValidFromUTC = original.ValidFromUTC,
            Reward = original.Reward
        };

        private static Mission ApplyMissionDtoToMissionObject(Mission original, MissionDto update) => original with
        {
            Title = update.Title,
            Description = update.Description,
            Steps = update.Steps,
            ValidToUTC = update.ValidToUTC,
            ValidFromUTC = update.ValidFromUTC,
            Reward = update.Reward
        };
    }
}