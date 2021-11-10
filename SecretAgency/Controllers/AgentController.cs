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
    [Route("api/agent")]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentDataService;

        private readonly ILogger<AgentController> _logger;

        public AgentController(ILogger<AgentController> logger, IAgentService agentDataService)
        {
            _logger = logger;
            _agentDataService = agentDataService;
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<Agent>> GetAgentByName(string username)
        {
            var agent = await _agentDataService.GetAgentByUsername(username);
            
            return agent == default ? NotFound(username) : Ok(agent);
        }
    }
}
