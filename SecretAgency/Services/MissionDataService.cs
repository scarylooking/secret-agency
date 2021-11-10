using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SecretAgency.Controllers;
using SecretAgency.Models;

namespace SecretAgency.Services
{
    public class MissionDataService : IMissionDataService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MissionDataService> _logger;

        private static Dictionary<Guid, Mission> Missions = new()
        {
            {
                Guid.Parse("a3b5f4db-2bd1-4cff-b398-f27daf76e000"),
                new Mission()
                {
                    Id = Guid.Parse("a3b5f4db-2bd1-4cff-b398-f27daf76e000"),
                    Title = "Mission 001",
                    Description = "This is a test mission, should you choose to accept it",
                    Steps = new[] { "Collect underpants", "?????", "Profit!" },
                    Reward = new[]
                    {
                        new Point {NumberOfPoints = 10, BucketType = "Experience"},
                        new Point {NumberOfPoints = 5, BucketType = "Ecosystem"},
                    }
                }
            },
            {
                Guid.Parse("3f6f6915-ae2b-4411-8d49-8de64da531df"),
                new Mission()
                {
                    Id = Guid.Parse("3f6f6915-ae2b-4411-8d49-8de64da531df"),
                    Title = "Mission 002",
                    Description = "This is still a test mission (should you choose to accept it), but a different one...",
                    Steps = new[] { "Send Spicy Memes to your parents", "Screen shot their response", "Profit! Or homelessness" },
                    Reward = new[]
                    {
                        new Point {NumberOfPoints = 25, BucketType = "Experience"},
                        new Point {NumberOfPoints = 10, BucketType = "Community"},
                    }
                }
            }
        };

        public MissionDataService(ILogger<MissionDataService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public bool UpdateMission(Guid id, Mission updatedMission)
        {
            if (id != updatedMission.Id) return false;

            Missions[updatedMission.Id] = updatedMission;

            return true;
        }

        public bool AddMission(Mission newMission)
        {
            if (MissionExists(newMission.Id)) return false;

            Missions.Add(newMission.Id, newMission);

            return true;
        }

        public IReadOnlyCollection<Mission> GetAllMissions() => Missions.Select(m => m.Value).ToArray();

        public Mission GetMissionById(Guid id) => MissionExists(id) ? Missions[id] : default;

        public bool MissionExists(Guid id) => Missions.ContainsKey(id);
    }
}
