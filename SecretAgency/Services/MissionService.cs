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

        public async Task<bool> DeleteMission(Guid id)
        {
            try
            {
                await _missionRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete mission with ID {MissionId}", id);
                return false;
            }
        }

        public async Task<Mission> UpdateMission(Mission mission)
        {
            try
            {
                await _missionRepository.Update(mission.Id, mission);
                return mission;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update mission with ID {MissionId} to state {@Mission}", mission.Id, mission);
                return default;
            }
        }

        public async Task<Mission> AddMission(Mission mission)
        {
            try
            {
                await _missionRepository.Create(mission);
                return mission;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create new mission with ID {MissionId} {@Mission}", mission.Id, mission);
                return default;
            }
        }

        public async Task<IReadOnlyCollection<Mission>> GetAllMissions()
        {
            try
            {
                var result = await _missionRepository.GetAll();
                return result.ToArray();

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get all missions");
                return default;
            }
        }

        public async Task<Mission> GetMissionById(Guid id)
        {
            try
            {
                var result = await _missionRepository.Get(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get mission {MissionId}", id);
                return default;
            }
        }

        public async Task<bool> MissionExists(Guid id)
        {
            return await _missionRepository.Exists(id);
        }
    }
}
