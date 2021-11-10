using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SecretAgency.Models;
using SecretAgency.Services.Interfaces;

namespace SecretAgency.Services
{
    public interface IMongoConnectionService
    {
        IMongoCollection<Agent> GetAgentCollection();
        IMongoCollection<Mission> GetMissionCollection();
    }

    public class MongoConnectionService : IMongoConnectionService
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        private IMongoCollection<Agent> _agentCollection;
        private IMongoCollection<Mission> _missionCollection;

        private readonly IConfiguration _configuration;
        private readonly ILogger<MongoConnectionService> _logger;
        
        public MongoConnectionService(ILogger<MongoConnectionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            _client = new MongoClient(configuration["Database:MongoDB:ConnectionString"]);
            _database = _client.GetDatabase(configuration["Database:MongoDB:DatabaseName"]);
        }

        public IMongoCollection<Agent> GetAgentCollection() => _agentCollection ??= _database.GetCollection<Agent>(_configuration["Database:MongoDB:CollectionNames:Agent"]);

        public IMongoCollection<Mission> GetMissionCollection() => _missionCollection ??= _database.GetCollection<Mission>(_configuration["Database:MongoDB:CollectionNames:Mission"]);
    }
}
