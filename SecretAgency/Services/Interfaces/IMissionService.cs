using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretAgency.Models;

namespace SecretAgency.Services.Interfaces
{
    public interface IMissionService
    {
        Task<bool> AddMission(Mission newMission);
        Task<bool> UpdateMission(Mission updatedMission);
        Task<IReadOnlyCollection<Mission>> GetAllMissions();
        Task<Mission> GetMissionById(Guid id);
        Task<bool> MissionExists(Guid id);
    }
}