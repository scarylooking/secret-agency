using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SecretAgency.Models;
using SecretAgency.Services.Interfaces;
using Serilog;

namespace SecretAgency.Services
{
    public class MissionService : IMissionService
    {
        private readonly IMongoCollection<Mission> _missions;

        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public MissionService(IConfiguration configuration, IMongoConnectionService mongoConnectionService)
        {
            _logger = Log.ForContext<MissionService>();
            _missions = mongoConnectionService.GetMissionCollection();

            _configuration = configuration;
        }

        public async Task<bool> DeleteMission(Guid missionId)
        {
            try
            {
                await _missions.FindOneAndDeleteAsync(m => m.Id == missionId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete mission with ID {MissionId}", missionId);
                return false;
            }
        }

        public async Task<Mission> UpdateMission(Mission updatedMission)
        {
            try
            {
                await _missions.ReplaceOneAsync(m => m.Id.Equals(updatedMission.Id), updatedMission);
                return updatedMission;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update mission with ID {MissionId} to state {@Mission}", updatedMission.Id, updatedMission);
                return default;
            }
        }

        public async Task<Mission> AddMission(Mission newMission)
        {
            try
            {
                await _missions.InsertOneAsync(newMission);
                return newMission;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create new mission with ID {MissionId} {@Mission}", newMission.Id, newMission);
                return default;
            }
        }

        public async Task<IReadOnlyCollection<Mission>> GetAllMissions()
        {
            try
            {
                var result = await _missions.FindAsync(m => true);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get all missions");
                return default;
            }
        }

        public async Task<Mission> GetMissionById(Guid missionId)
        {
            try
            {
                var result = await _missions.FindAsync(m => m.Id.Equals(missionId));
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get mission {MissionId}", missionId);
                return default;
            }
        }

        public async Task<bool> MissionExists(Guid missionId)
        {
            var count = await _missions.CountDocumentsAsync(m => m.Id.Equals(missionId));
            return count > 0;
        }
    }
}
