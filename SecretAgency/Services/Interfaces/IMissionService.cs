using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretAgency.Models;

namespace SecretAgency.Services.Interfaces
{
    public interface IMissionService
    {
        Task<Mission> AddMission(Mission mission);
        Task<bool> DeleteMission(Guid missionId);
        Task<Mission> UpdateMission(Mission updatedMission);
        Task<IReadOnlyCollection<Mission>> GetAllMissions();
        Task<Mission> GetMissionById(Guid id);
        Task<bool> MissionExists(Guid id);
    }
}