using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<IEnumerable<Mission>> Get()
        {
            return await _missionService.GetAllMissions();
        }

        [HttpGet]
        [Route("{id:guid}")]

        public async Task<ActionResult<Mission>> GetMissionById(Guid id)
        {
            var mission = await _missionService.GetMissionById(id);

            return mission == default ? NotFound(id) : Ok(mission);
        }

        [HttpPut]
        public async Task<ActionResult<Mission>> UpdateMission([FromBody] Mission updatedMission)
        {
            var result = await _missionService.UpdateMission(updatedMission);
            
            return result ? Ok(updatedMission) : BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<Mission>> AddMission([FromBody] Mission newMission)
        {
            var result = await _missionService.AddMission(newMission);

            return result ? Ok(newMission) : Conflict(newMission.Id);
        }
    }
}
