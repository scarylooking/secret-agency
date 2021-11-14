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

        public async Task<IResult<bool>> Delete(Guid id)
        {
            try
            {
                var result = await _missionCollection.DeleteOneAsync(m => m.Id == id);
                return RepositoryResult.Success(result.DeletedCount > 0);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete mission with ID {MissionID}", id);
                return RepositoryResult.Failure<bool>();
            }
        }

        public async Task<IResult<IEnumerable<Mission>>> GetAll()
        {
            try
            {
                var result = await _missionCollection.FindAsync(m => true);

                return RepositoryResult.Success(result.ToEnumerable());

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get all missions");
                return RepositoryResult.Failure<IEnumerable<Mission>>();
            }
        }

        public async Task<IResult<Mission>> Get(Guid id)
        {
            try
            {
                var result = await _missionCollection.FindAsync(m => m.Id.Equals(id));
                return RepositoryResult.Success(result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get mission with ID {MissionId}", id);
                return RepositoryResult.Failure<Mission>();
            }
        }

        public async Task<IResult<Mission>> Create(Mission mission)
        {
            try
            {
                await _missionCollection.InsertOneAsync(mission);
                return RepositoryResult.Success(mission);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create new mission {@Mission}", mission);
                return RepositoryResult.Failure<Mission>();
            }
        }

        public async Task<IResult<Mission>> Update(Guid id, Mission mission)
        {
            try
            {
                await _missionCollection.ReplaceOneAsync(m => m.Id.Equals(id), mission);
                return RepositoryResult.Success(mission);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update mission {MissionId}: {NewState}", id, mission);
                return RepositoryResult.Failure<Mission>();
            }
        }

        public async Task<IResult<bool>> Exists(Guid id)
        {
            try
            {
                var result = await _missionCollection.CountDocumentsAsync(m => m.Id.Equals(id));
                return RepositoryResult.Success(result > 0);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to determine if mission with ID {MissionId} exists", id);
                return RepositoryResult.Failure<bool>();
            }
        }
    }
}
