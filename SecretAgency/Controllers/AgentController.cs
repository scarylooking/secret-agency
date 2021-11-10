using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretAgency.Models;
using SecretAgency.Services;

namespace SecretAgency.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/agent")]
    public class AgentController : ControllerBase
    {
        private readonly IAgentDataService _agentDataService;

        private readonly ILogger<AgentController> _logger;

        public AgentController(ILogger<AgentController> logger, IAgentDataService agentDataService)
        {
            _logger = logger;
            _agentDataService = agentDataService;
        }

        [HttpGet]
        [Route("{username}")]
        public ActionResult<Agent> GetAgentByName(string username)
        {
            var agent = _agentDataService.GetAgentByUsername(username);

            return agent == default ? NotFound(username) : Ok(agent);
        }
    }
}
