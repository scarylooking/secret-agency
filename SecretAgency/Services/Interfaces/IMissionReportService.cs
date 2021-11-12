using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretAgency.Constants;
using SecretAgency.Models;

namespace SecretAgency.Services.Interfaces
{
    public interface IMissionReportService
    {
        Task<bool> DeleteMissionReport(Guid id);
        Task<MissionReport> UpdateMissionReport(MissionReport missionReport);
        Task<MissionReport> AddMissionReport(MissionReport missionReport);
        Task<IReadOnlyCollection<MissionReport>> GetPendingReports();
        Task<MissionReport> GetMissionReportById(Guid id);
        Task<bool> MissionReportExists(Guid id);
        Task<MissionReport> SetMissionState(Guid id, MissionReportApprovalState state);
    }
}