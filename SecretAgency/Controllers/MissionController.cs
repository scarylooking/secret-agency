using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretAgency.Models;
using SecretAgency.Services;

namespace SecretAgency.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/mission")]
    public class MissionController : ControllerBase
    {
        private readonly IMissionDataService _missionDataService;

        private readonly ILogger<MissionController> _logger;

        public MissionController(ILogger<MissionController> logger, IMissionDataService missionDataService)
        {
            _logger = logger;
            _missionDataService = missionDataService;
        }

        [HttpGet]
        public IEnumerable<Mission> Get()
        {
            return _missionDataService.GetAllMissions();
        }

        [HttpGet]
        [Route("{id:guid}")]

        public ActionResult<Mission> GetMissionById(Guid id)
        {
            var mission = _missionDataService.GetMissionById(id);

            return mission == default ? NotFound(id) : Ok(mission);
        }

        [HttpPost]
        [Route("{id:guid}")]

        public ActionResult<Mission> UpdateMission(Guid id, [FromBody] Mission updatedMission)
        {
            if (!_missionDataService.MissionExists(id)) return NotFound(updatedMission.Id);

            var result = _missionDataService.UpdateMission(id, updatedMission);
            
            return result ? Ok(updatedMission) : BadRequest();
        }

        [HttpPost]
        public ActionResult<Mission> AddMission([FromBody] Mission newMission)
        {
            var result = _missionDataService.AddMission(newMission);

            return result ? Ok(newMission) : Conflict(newMission.Id);
        }
    }
}
