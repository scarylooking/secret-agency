using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretAgency.Constants;
using SecretAgency.Models;
using SecretAgency.Services;

namespace SecretAgency.Repositories.Interfaces
{
    public interface IMissionReportRepository
    {
        Task<IResult<bool>> Delete(Guid id);
        Task<IResult<IEnumerable<MissionReport>>> GetAllInState(MissionReportApprovalState state);
        Task<IResult<MissionReport>> Get(Guid id);
        Task<IResult<MissionReport>> Create(MissionReport missionReport);
        Task<IResult<MissionReport>> Update(Guid id, MissionReport missionReport);
        Task<IResult<bool>> Exists(Guid id);
    }

}