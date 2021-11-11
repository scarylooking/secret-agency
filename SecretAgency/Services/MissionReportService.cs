using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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

        public MissionReportService(IConfiguration configuration, IMissionReportRepository missionReportRepository)
        {
            _logger = Log.ForContext<MissionReportService>();
            _missionReportRepository = missionReportRepository;

            _configuration = configuration;
        }

        public async Task<bool> DeleteMissionReport(Guid id)
        {
            try
            {
                await _missionReportRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete mission report with ID {MissionReportId}", id);
                return false;
            }
        }

        public async Task<MissionReport> UpdateMissionReport(MissionReport missionReport)
        {
            try
            {
                await _missionReportRepository.Update(missionReport.Id, missionReport);
                return missionReport;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update mission report with ID {MissionReportId} to state {@MissionReport}", missionReport.Id, missionReport);
                return default;
            }
        }

        public async Task<MissionReport> AddMissionReport(MissionReport missionReport)
        {
            try
            {
                await _missionReportRepository.Create(missionReport);
                return missionReport;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create new mission report with ID {MissionReportId} {@MissionReport}", missionReport.Id, missionReport);
                return default;
            }
        }

        public async Task<IReadOnlyCollection<MissionReport>> GetPendingReports()
        {
            try
            {
                var result = await _missionReportRepository.GetAllInState(MissionReportApprovalState.Pending);
                return result.ToArray();

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get pending mission reports");
                return default;
            }
        }

        public async Task<MissionReport> GetMissionReportById(Guid id)
        {
            try
            {
                var result = await _missionReportRepository.Get(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get mission report {MissionReportId}", id);
                return default;
            }
        }

        public async Task<MissionReport> SetMissionState(Guid id, MissionReportApprovalState state)
        {
            try
            {
                var missionReport = await _missionReportRepository.Get(id);

                var updatedMissionReport = missionReport with {State = state};

                var result = await _missionReportRepository.Update(id, updatedMissionReport);

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to set state for mission report {MissionReportId} to {NewState}", id, state);
                return default;
            }
        }

        public async Task<bool> MissionReportExists(Guid id)
        {
            return await _missionReportRepository.Exists(id);
        }


    }
}
