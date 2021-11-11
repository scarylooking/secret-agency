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
        public async Task<IEnumerable<Mission>> Get()
        {
            return await _missionService.GetAllMissions();
        }

        [HttpGet]
        [Route("{missionId:guid}")]
        public async Task<ActionResult<Mission>> GetMissionById(Guid missionId)
        {
            if (!await _missionService.MissionExists(missionId))
            {
                return NotFound();
            }

            var mission = await _missionService.GetMissionById(missionId);

            return mission != default ? Ok(mission) : BadRequest(missionId);
        }

        [HttpPut]
        public async Task<ActionResult<Mission>> UpdateMission([FromBody] Mission mission)
        {
            if (!await _missionService.MissionExists(mission.Id))
            {
                return NotFound();
            }

            var result = await _missionService.UpdateMission(mission);

            return result != default ? Ok(mission) : BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<Mission>> AddMission([FromBody] Mission mission)
        {
            if (await _missionService.MissionExists(mission.Id))
            {
                return Conflict();
            }

            var result = await _missionService.AddMission(mission);

            return result != default ? Ok(mission) : BadRequest();
        }

        [HttpDelete]
        [Route("{missionId:guid}")]
        public async Task<ActionResult<Mission>> DeleteMission(Guid missionId)
        {
            if (!await _missionService.MissionExists(missionId))
            {
                return NotFound();
            }

            var result = await _missionService.DeleteMission(missionId);

            return result ? Ok() : BadRequest();
        }
    }
}
