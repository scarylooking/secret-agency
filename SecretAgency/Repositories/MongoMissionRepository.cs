using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SecretAgency.Models;
using SecretAgency.Services;
using Serilog;

namespace SecretAgency.Repositories
{
    public class MongoMissionRepository : IMissionRepository
    {
        private readonly IMongoCollection<Mission> _missionCollection;

        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public MongoMissionRepository(IConfiguration configuration, IMongoConnectionService mongoConnectionService)
        {
            _logger = Log.ForContext<MongoMissionRepository>();
            _missionCollection = mongoConnectionService.GetMissionCollection();

            _configuration = configuration;
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _missionCollection.DeleteOneAsync(m => m.Id == id);

            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Mission>> GetAll()
        {
            var result = await _missionCollection.FindAsync(m => true);
            return result.ToList();
        }

        public async Task<Mission> Get(Guid id)
        {
            var result = await _missionCollection.FindAsync(m => m.Id.Equals(id));
            return result.FirstOrDefault();
        }

        public async Task<Mission> Create(Mission mission)
        {
            await _missionCollection.InsertOneAsync(mission);
            return mission;
        }

        public async Task<Mission> Update(Guid id, Mission mission)
        {
            await _missionCollection.ReplaceOneAsync(m => m.Id.Equals(id), mission);
            return mission;
        }

        public async Task<bool> Exists(Guid id)
        {
            var result = await _missionCollection.CountDocumentsAsync(m => m.Id.Equals(id));
            return result > 0;
        }
    }
}
