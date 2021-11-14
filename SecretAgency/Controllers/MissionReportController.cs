using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretAgency.Constants;
using SecretAgency.Models;
using SecretAgency.Services;
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
        public async Task<ActionResult<IResult<IEnumerable<MissionReport>>>> GetPending()
        {
            var result = await _missionReportService.GetPendingReports();

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            return BadRequest(ControllerResult.Failure<IEnumerable<MissionReport>>(Errors.DatabaseError));
        }

        [HttpGet]
        [Route("{missionReportId:guid}")]
        public async Task<ActionResult<IResult<MissionReport>>> GetMissionReportById(Guid missionReportId)
        {
            var result = await _missionReportService.GetMissionReportById(missionReportId);

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            if (result.Errors.Contains(Errors.MissionReportNotFound))
            {
                return NotFound(ControllerResult.Failure<MissionReport>(Errors.MissionReportNotFound));
            }

            return BadRequest(ControllerResult.Failure<MissionReport>(Errors.DatabaseError));

        }

        [HttpPatch]
        [Route("{missionReportId:guid}/approve")]
        public async Task<ActionResult<IResult<MissionReport>>> ApproveMissionReport(Guid missionReportId)
        {
            var result = await _missionReportService.SetMissionState(missionReportId, MissionReportApprovalState.Approved);

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            if (result.Errors.Contains(Errors.MissionReportNotFound))
            {
                return NotFound(ControllerResult.Failure<MissionReport>(Errors.MissionReportNotFound));
            }

            return BadRequest(ControllerResult.Failure<MissionReport>(Errors.DatabaseError));
        }

        [HttpPatch]
        [Route("{missionReportId:guid}/reject")]
        public async Task<ActionResult<IResult<MissionReport>>> RejectMissionReport(Guid missionReportId)
        {
            var result = await _missionReportService.SetMissionState(missionReportId, MissionReportApprovalState.Rejected);

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            if (result.Errors.Contains(Errors.MissionReportNotFound))
            {
                return NotFound(ControllerResult.Failure<MissionReport>(Errors.MissionReportNotFound));
            }

            return BadRequest(ControllerResult.Failure<MissionReport>(Errors.DatabaseError));
        }

        [HttpPut]
        [Route("{missionReportId:guid}")]
        public async Task<ActionResult<IResult<MissionReport>>> UpdateMissionReport(Guid missionReportId, [FromBody] MissionReportDto missionReport)
        {
            var result = await _missionReportService.UpdateMissionReport(missionReportId, missionReport);

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            if (result.Errors.Contains(Errors.MissionReportNotFound))
            {
                return NotFound(ControllerResult.Failure<MissionReport>(Errors.MissionReportNotFound));
            }

            return BadRequest(ControllerResult.Failure<MissionReport>(Errors.DatabaseError));
        }

        [HttpPost]
        public async Task<ActionResult<IResult<MissionReport>>> AddMissionReport([FromBody] MissionReportDto missionReportDto)
        {
            var result = await _missionReportService.AddMissionReport(missionReportDto);

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            if (result.Errors.Contains(Errors.MissionAlreadyExists))
            {
                return Conflict(ControllerResult.Failure<MissionReport>(Errors.MissionAlreadyExists));
            }

            return BadRequest(ControllerResult.Failure<MissionReport>(Errors.DatabaseError));
        }

        [HttpDelete]
        [Route("{missionReportId:guid}")]
        public async Task<ActionResult<IResult<bool>>> DeleteMission(Guid missionReportId)
        {
            var result = await _missionReportService.DeleteMissionReport(missionReportId);

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            if (result.Errors.Contains(Errors.MissionReportNotFound))
            {
                return NotFound(ControllerResult.Failure<MissionReport>(Errors.MissionReportNotFound));
            }

            return BadRequest(ControllerResult.Failure<bool>(Errors.DatabaseError));
        }
    }
}
