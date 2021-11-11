using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretAgency.Models;
using SecretAgency.Services.Interfaces;

namespace SecretAgency.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/missionreport")]
    public class MissionReportController : ControllerBase
    {
        private readonly IMissionReportService _missionReportService;
        private readonly ILogger<MissionReportController> _logger;

        public MissionReportController(ILogger<MissionReportController> logger, IMissionReportService missionReportService)
        {
            _logger = logger;
            _missionReportService = missionReportService;
        }

        [HttpGet]
        [Route("pending")]
        public async Task<IEnumerable<MissionReport>> GetPending()
        {
            return await _missionReportService.GetPendingReports();
        }

        [HttpGet]
        [Route("{missionReportId:guid}")]
        public async Task<ActionResult<MissionReport>> GetMissionReportById(Guid missionReportId)
        {
            if (!await _missionReportService.MissionReportExists(missionReportId))
            {
                return NotFound();
            }

            var missionReport = await _missionReportService.GetMissionReportById(missionReportId);

            return missionReport != default ? Ok(missionReport) : BadRequest();
        }

        [HttpPatch]
        [Route("{missionReportId:guid}/approve")]
        public async Task<ActionResult<MissionReport>> ApproveMissionReport(Guid missionReportId)
        {
            if (!await _missionReportService.MissionReportExists(missionReportId))
            {
                return NotFound();
            }

            var missionReport = await _missionReportService.SetMissionState(missionReportId, MissionReportApprovalState.Approved);

            return missionReport != default ? Ok(missionReport) : BadRequest();
        }

        [HttpPatch]
        [Route("{missionReportId:guid}/reject")]
        public async Task<ActionResult<MissionReport>> RejectMissionReport(Guid missionReportId)
        {
            if (!await _missionReportService.MissionReportExists(missionReportId))
            {
                return NotFound();
            }

            var missionReport = await _missionReportService.SetMissionState(missionReportId, MissionReportApprovalState.Rejected);

            return missionReport != default ? Ok(missionReport) : BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult<MissionReport>> UpdateMission([FromBody] MissionReport missionReport)
        {
            if (!await _missionReportService.MissionReportExists(missionReport.Id))
            {
                return NotFound();
            }

            var result = await _missionReportService.UpdateMissionReport(missionReport);

            return result != default ? Ok(missionReport) : BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<MissionReport>> AddMission([FromBody] MissionReport missionReport)
        {
            if (await _missionReportService.MissionReportExists(missionReport.Id))
            {
                return Conflict();
            }

            var result = await _missionReportService.AddMissionReport(missionReport);

            return result != default ? Ok(missionReport) : BadRequest();
        }

        [HttpDelete]
        [Route("{missionId:guid}")]
        public async Task<ActionResult<Mission>> DeleteMission(Guid missionReportId)
        {
            if (!await _missionReportService.MissionReportExists(missionReportId))
            {
                return NotFound();
            }

            var result = await _missionReportService.DeleteMissionReport(missionReportId);

            return result ? Ok() : BadRequest();
        }
    }
}
