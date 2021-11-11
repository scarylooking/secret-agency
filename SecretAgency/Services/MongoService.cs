using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SecretAgency.Models;

namespace SecretAgency.Services
{
    public interface IMongoConnectionService
    {
        IMongoCollection<Mission> GetMissionCollection();
        IMongoCollection<MissionReport> GetMissionReportCollection();
    }

    public class MongoConnectionService : IMongoConnectionService
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<Mission> _missionCollection;
        private IMongoCollection<MissionReport> _missionReportCollection;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MongoConnectionService> _logger;

        public MongoConnectionService(ILogger<MongoConnectionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            var client = new MongoClient(configuration["Database:MongoDB:ConnectionString"]);
            _database = client.GetDatabase(configuration["Database:MongoDB:DatabaseName"]);
        }

        public IMongoCollection<Mission> GetMissionCollection() => _missionCollection ??= _database.GetCollection<Mission>(_configuration["Database:MongoDB:CollectionNames:Mission"]);
        public IMongoCollection<MissionReport> GetMissionReportCollection() => _missionReportCollection ??= _database.GetCollection<MissionReport>(_configuration["Database:MongoDB:CollectionNames:MissionReport"]);
    }
}
