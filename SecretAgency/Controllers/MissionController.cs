using System;
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
    [Route("api/mission")]
    public class MissionController : ControllerBase
    {
        private readonly IMissionService _missionService;
        private readonly ILogger<MissionController> _logger;

        public MissionController(ILogger<MissionController> logger, IMissionService missionService)
        {
            _logger = logger;
            _missionService = missionService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _missionService.GetAllMissions();

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            return BadRequest(ControllerResult.Failure<Mission>(Errors.DatabaseError));
        }

        [HttpGet]
        [Route("{missionId:guid}")]
        public async Task<ActionResult<IResult<Mission>>> GetMissionById(Guid missionId)
        {
            var result = await _missionService.GetMissionById(missionId);

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            if (result.Errors.Contains(Errors.MissionNotFound))
            {
                return NotFound(ControllerResult.Failure<Mission>(Errors.MissionNotFound));
            }

            return BadRequest(ControllerResult.Failure<Mission>(Errors.DatabaseError));
        }

        [HttpPut]
        [Route("{missionId:guid}")]
        public async Task<ActionResult<IResult<Mission>>> UpdateMission(Guid missionId, [FromBody] MissionDto mission)
        {
            var result = await _missionService.UpdateMission(missionId, mission);

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            if (result.Errors.Contains(Errors.MissionNotFound))
            {
                return NotFound(ControllerResult.Failure<Mission>(Errors.MissionNotFound));
            }

            return BadRequest(ControllerResult.Failure<Mission>(Errors.DatabaseError));
        }

        [HttpPost]
        public async Task<ActionResult<IResult<Mission>>> AddMission([FromBody] MissionDto missionDto)
        {
            var result = await _missionService.AddMission(missionDto);

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            if (result.Errors.Contains(Errors.MissionAlreadyExists))
            {
                return Conflict(ControllerResult.Failure<Mission>(Errors.MissionAlreadyExists));
            }

            return BadRequest(ControllerResult.Failure<Mission>(Errors.DatabaseError));
        }

        [HttpDelete]
        [Route("{missionId:guid}")]
        public async Task<ActionResult<IResult<bool>>> DeleteMission(Guid missionId)
        {
            var result = await _missionService.DeleteMission(missionId);

            if (result.IsSuccessful) return Ok(ControllerResult.Success(result.Value));

            if (result.Errors.Contains(Errors.MissionNotFound))
            {
                return NotFound(ControllerResult.Failure<bool>(Errors.MissionNotFound));
            }

            return BadRequest(ControllerResult.Failure<bool>(Errors.DatabaseError));
        }
    }
}
