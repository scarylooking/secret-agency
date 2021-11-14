using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretAgency.Constants;
using SecretAgency.Models;

namespace SecretAgency.Services.Interfaces
{
    public interface IMissionReportService
    {
        Task<IResult<bool>> DeleteMissionReport(Guid id);
        Task<IResult<MissionReport>> UpdateMissionReport(Guid id, MissionReportDto missionReport);
        Task<IResult<MissionReport>> AddMissionReport(MissionReportDto missionReport);
        Task<IResult<IEnumerable<MissionReport>>> GetPendingReports();
        Task<IResult<MissionReport>> GetMissionReportById(Guid id);
        Task<IResult<MissionReport>> SetMissionState(Guid id, MissionReportApprovalState state);
        Task<IResult<bool>> MissionReportExists(Guid id);
    }
}