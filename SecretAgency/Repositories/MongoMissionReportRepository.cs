using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SecretAgency.Constants;
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

        public async Task<IResult<bool>> Delete(Guid id)
        {
            try
            {
                var result = await _missionReportCollection.DeleteOneAsync(m => m.Id == id);
                return RepositoryResult.Success(result.DeletedCount > 0);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete mission report with ID {MissionReportID}", id);
                return RepositoryResult.Failure<bool>();
            }
        }

        public async Task<IResult<IEnumerable<MissionReport>>> GetAllInState(MissionReportApprovalState state)
        {
            try
            {
                var result = await _missionReportCollection.FindAsync(m => m.State == state);
                return RepositoryResult.Success(result.ToEnumerable());

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get mission reports in state {State}", state);
                return RepositoryResult.Failure<IEnumerable<MissionReport>>();
            }
        }

        public async Task<IResult<MissionReport>> Get(Guid id)
        {
            try
            {
                var result = await _missionReportCollection.FindAsync(m => m.Id.Equals(id));
                return RepositoryResult.Success(result.FirstOrDefault());

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get mission report with ID {MissionReportId}", id);
                return RepositoryResult.Failure<MissionReport>();
            }
        }

        public async Task<IResult<MissionReport>> Create(MissionReport missionReport)
        {
            try
            {
                await _missionReportCollection.InsertOneAsync(missionReport);
                return RepositoryResult.Success(missionReport);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create new mission report {@MissionReport}", missionReport);
                return RepositoryResult.Failure<MissionReport>();
            }
        }

        public async Task<IResult<MissionReport>> Update(Guid id, MissionReport missionReport)
        {
            try
            {
                await _missionReportCollection.ReplaceOneAsync(m => m.Id.Equals(id), missionReport);
                return RepositoryResult.Success(missionReport);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update mission report {MissionReportId}: {NewState}", id, missionReport);
                return RepositoryResult.Failure<MissionReport>();
            }
        }

        public async Task<IResult<bool>> Exists(Guid id)
        {
            try
            {
                var result = await _missionReportCollection.CountDocumentsAsync(m => m.Id.Equals(id));
                return RepositoryResult.Success(result > 0);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to determine if mission report with ID {MissionReportId} exists", id);
                return RepositoryResult.Failure<bool>();
            }
        }
    }
}