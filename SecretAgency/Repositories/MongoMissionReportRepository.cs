using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SecretAgency.Models;
using SecretAgency.Repositories.Interfaces;
using SecretAgency.Services;
using Serilog;

namespace SecretAgency.Repositories
{
    public class MongoMissionReportRepository : IMissionReportRepository
    {
        private readonly IMongoCollection<MissionReport> _missionReportCollection;

        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public MongoMissionReportRepository(IConfiguration configuration, IMongoConnectionService mongoConnectionService)
        {
            _logger = Log.ForContext<MongoMissionReportRepository>();
            _missionReportCollection = mongoConnectionService.GetMissionReportCollection();

            _configuration = configuration;
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _missionReportCollection.DeleteOneAsync(m => m.Id == id);

            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<MissionReport>> GetAllInState(MissionReportApprovalState state)
        {
            var result = await _missionReportCollection.FindAsync(m => m.State == state);
            return result.ToList();
        }

        public async Task<MissionReport> Get(Guid id)
        {
            var result = await _missionReportCollection.FindAsync(m => m.Id.Equals(id));
            return result.FirstOrDefault();
        }

        public async Task<MissionReport> Create(MissionReport mission)
        {
            await _missionReportCollection.InsertOneAsync(mission);
            return mission;
        }

        public async Task<MissionReport> Update(Guid id, MissionReport mission)
        {
            await _missionReportCollection.ReplaceOneAsync(m => m.Id.Equals(id), mission);
            return mission;
        }

        public async Task<bool> Exists(Guid id)
        {
            var result = await _missionReportCollection.CountDocumentsAsync(m => m.Id.Equals(id));
            return result > 0;
        }
    }
}
