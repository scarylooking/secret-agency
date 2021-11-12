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

            return Ok(new Response<IReadOnlyCollection<Mission>>(result).AsSuccess());
        }

        [HttpGet]
        [Route("{missionId:guid}")]
        public async Task<ActionResult> GetMissionById(Guid missionId)
        {
            if (!await _missionService.MissionExists(missionId))
            {
                return NotFound(new EmptyResponse().AsError(Errors.MissionNotFound));
            }

            var result = await _missionService.GetMissionById(missionId);

            return result != default 
                ? Ok(new Response<Mission>(result).AsSuccess()) 
                : BadRequest(new EmptyResponse().AsError(Errors.UnknownError));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMission([FromBody] Mission mission)
        {
            if (!await _missionService.MissionExists(mission.Id))
            {
                return NotFound(new EmptyResponse().AsError(Errors.MissionNotFound));
            }

            var result = await _missionService.UpdateMission(mission);

            return result != default 
                ? Ok(new Response<Mission>(result).AsSuccess()) 
                : BadRequest(new EmptyResponse().AsError(Errors.UnknownError));
        }

        [HttpPost]
        public async Task<ActionResult> AddMission([FromBody] Mission mission)
        {
            if (await _missionService.MissionExists(mission.Id))
            {
                return Conflict(new EmptyResponse().AsError(Errors.MissionAlreadyExists));
            }
            
            var result = await _missionService.AddMission(mission);

            return result != default 
                ? Ok(new Response<Mission>(result).AsSuccess()) 
                : BadRequest(new EmptyResponse().AsError(Errors.UnknownError));
        }

        [HttpDelete]
        [Route("{missionId:guid}")]
        public async Task<ActionResult> DeleteMission(Guid missionId)
        {
            if (!await _missionService.MissionExists(missionId))
            {
                return NotFound(new EmptyResponse().AsError(Errors.MissionNotFound));
            }

            var result = await _missionService.DeleteMission(missionId);

            return result != default 
                ? Ok(new EmptyResponse().AsSuccess()) 
                : BadRequest(new EmptyResponse().AsError(Errors.UnknownError));
        }
    }
}
