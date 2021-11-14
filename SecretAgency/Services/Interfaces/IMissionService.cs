using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretAgency.Models;

namespace SecretAgency.Services.Interfaces
{
    public interface IMissionService
    {
        Task<IResult<bool>> DeleteMission(Guid id);
        Task<IResult<Mission>> UpdateMission(Guid id, MissionDto missionDto);
        Task<IResult<Mission>> AddMission(MissionDto mission);
        Task<IResult<IEnumerable<Mission>>> GetAllMissions();
        Task<IResult<Mission>> GetMissionById(Guid id);
        Task<IResult<bool>> MissionExists(Guid id);
    }
}