using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SecretAgency.Models;
using SecretAgency.Services.Interfaces;

namespace SecretAgency.Services
{
    public class MissionService : IMissionService
    {
        private readonly IMongoCollection<Mission> _missions;

        private readonly IConfiguration _configuration;
        private readonly ILogger<MissionService> _logger;

        public MissionService(ILogger<MissionService> logger, IConfiguration configuration, IMongoConnectionService mongoConnectionService)
        {
            _missions = mongoConnectionService.GetMissionCollection();

            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> UpdateMission(Mission updatedMission)
        {
            if (!await MissionExists(updatedMission.Id)) return false;

            await _missions.ReplaceOneAsync(m => m.Id.Equals(updatedMission.Id), updatedMission);

            return true;
        }

        public async Task<bool> AddMission(Mission newMission)
        {
            if (await MissionExists(newMission.Id)) return false;

            await _missions.InsertOneAsync(newMission);

            return true;
        }

        public async Task<IReadOnlyCollection<Mission>> GetAllMissions() => (await _missions.FindAsync(m => true)).ToList();

        public async Task<Mission> GetMissionById(Guid id) => (await _missions.FindAsync(m => m.Id.Equals(id))).FirstOrDefault();

        public async Task<bool> MissionExists(Guid id)
        {
            var count = await _missions.CountDocumentsAsync(m => m.Id.Equals(id));

            return count > 0;
        }
    }
}
