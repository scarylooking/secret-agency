using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretAgency.Models;

namespace SecretAgency.Repositories.Interfaces
{
    public interface IMissionReportRepository
    {
        Task<bool> Delete(Guid id);
        Task<IEnumerable<MissionReport>> GetAllInState(MissionReportApprovalState state);
        Task<MissionReport> Get(Guid id);
        Task<MissionReport> Create(MissionReport mission);
        Task<MissionReport> Update(Guid id, MissionReport mission);
        Task<bool> Exists(Guid id);
    }
}