using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretAgency.Constants;
using SecretAgency.Models;
using SecretAgency.Services.Interfaces;

namespace SecretAgency.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/missionReport")]
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
        public async Task<ActionResult> GetPending()
        {
            var result = await _missionReportService.GetPendingReports();

            return Ok(new Response<IReadOnlyCollection<MissionReport>>(result).AsSuccess());
        }

        [HttpGet]
        [Route("{missionReportId:guid}")]
        public async Task<ActionResult<MissionReport>> GetMissionReportById(Guid missionReportId)
        {
            if (!await _missionReportService.MissionReportExists(missionReportId))
            {
                return NotFound(new EmptyResponse().AsError(Errors.MissionReportNotFound));
            }

            var result = await _missionReportService.GetMissionReportById(missionReportId);

            return result != default
                ? Ok(new Response<MissionReport>(result).AsSuccess())
                : BadRequest(new EmptyResponse().AsError(Errors.UnknownError));
        }

        [HttpPatch]
        [Route("{missionReportId:guid}/approve")]
        public async Task<ActionResult<MissionReport>> ApproveMissionReport(Guid missionReportId)
        {
            if (!await _missionReportService.MissionReportExists(missionReportId))
            {
                return NotFound(new EmptyResponse().AsError(Errors.MissionReportNotFound));
            }

            var result = await _missionReportService.SetMissionState(missionReportId, MissionReportApprovalState.Approved);

            return result != default
                ? Ok(new Response<Guid>(missionReportId).AsSuccess())
                : BadRequest(new EmptyResponse().AsError(Errors.UnknownError));
        }

        [HttpPatch]
        [Route("{missionReportId:guid}/reject")]
        public async Task<ActionResult<MissionReport>> RejectMissionReport(Guid missionReportId)
        {
            if (!await _missionReportService.MissionReportExists(missionReportId))
            {
                return NotFound(new EmptyResponse().AsError(Errors.MissionReportNotFound));
            }

            var result = await _missionReportService.SetMissionState(missionReportId, MissionReportApprovalState.Rejected);

            return result != default
                ? Ok(new Response<Guid>(missionReportId).AsSuccess())
                : BadRequest(new EmptyResponse().AsError(Errors.UnknownError));
        }

        [HttpPut]
        [Route("{missionReportId:guid}")]
        public async Task<ActionResult<MissionReport>> UpdateMissionReport(Guid missionReportId, [FromBody] MissionReportDto missionReport)
        {
            if (!await _missionReportService.MissionReportExists(missionReportId))
            {
                return NotFound(new EmptyResponse().AsError(Errors.MissionReportNotFound));
            }

            var originalMissionReport = await _missionReportService.GetMissionReportById(missionReportId);

            var updatedMissionReport = UpdateMissionReportWithDto(originalMissionReport, missionReport);

            var result = await _missionReportService.UpdateMissionReport(updatedMissionReport);

            return result != default
                ? Ok(new Response<MissionReport>(result).AsSuccess())
                : BadRequest(new EmptyResponse().AsError(Errors.UnknownError));
        }

        [HttpPost]
        public async Task<ActionResult<MissionReport>> AddMission([FromBody] MissionReportDto missionReportDto)
        {
            var missionReport = CreateMissionReportFromDto(missionReportDto);

            if (await _missionReportService.MissionReportExists(missionReport.Id))
            {
                return Conflict(new EmptyResponse().AsError(Errors.MissionReportAlreadyExists));
            }

            var result = await _missionReportService.AddMissionReport(missionReport);

            return result != default
                ? Ok(new Response<MissionReport>(result).AsSuccess())
                : BadRequest(new EmptyResponse().AsError(Errors.UnknownError));
        }

        [HttpDelete]
        [Route("{missionId:guid}")]
        public async Task<ActionResult<Mission>> DeleteMission(Guid missionReportId)
        {
            if (!await _missionReportService.MissionReportExists(missionReportId))
            {
                return NotFound(new EmptyResponse().AsError(Errors.MissionReportNotFound));
            }

            var result = await _missionReportService.DeleteMissionReport(missionReportId);

            return result != default
                ? Ok(new Response<Guid>(missionReportId).AsSuccess())
                : BadRequest(new EmptyResponse().AsError(Errors.UnknownError));
        }

        private static MissionReport CreateMissionReportFromDto(MissionReportDto original) => new MissionReport()
        {
            MissionId = original.MissionId,
            FieldNotes = original.FieldNotes,
            TwitterHandle = original.TwitterHandle,
            Password = original.Password
        };

        private static MissionReport UpdateMissionReportWithDto(MissionReport original, MissionReportDto update) => original with
        {
            MissionId = update.MissionId,
            FieldNotes = update.FieldNotes,
            TwitterHandle = update.TwitterHandle,
            Password = update.Password
        };
    }
}
