using System;
using System.Collections.Generic;
using SecretAgency.Models;

namespace SecretAgency.Services
{
    public interface IMissionDataService
    {
        bool MissionExists(Guid id);
        bool AddMission(Mission newMission);
        bool UpdateMission(Guid id, Mission updatedMission);
        public IReadOnlyCollection<Mission> GetAllMissions();
        public Mission GetMissionById(Guid id);
    }
}