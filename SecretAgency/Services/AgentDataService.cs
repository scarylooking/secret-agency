using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SecretAgency.Controllers;
using SecretAgency.Models;

namespace SecretAgency.Services
{
    public class AgentDataService : IAgentDataService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AgentDataService> _logger;

        private static readonly Dictionary<string, Agent> Agents = new()
        {
            {
                "alpaca",
                new Agent
                {
                    Username = "alpaca",
                    Balances = new Dictionary<string, int>()
                    {
                        { "experience", 100 }
                    }
                }
            }
        };

        public AgentDataService(ILogger<AgentDataService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public bool CreateNewAgent(Agent newAgent)
        {
            if (!Agents.ContainsKey(newAgent.Username)) return false;

            Agents.Add(newAgent.Username, newAgent);

            return true;
        }

        public bool AddPointsToAgent(string username, string balanceName, int points)
        {
            if (!Agents.ContainsKey(username)) return false;

            if (!Agents[username].Balances.ContainsKey(balanceName))
            {
                Agents[username].Balances.Add(balanceName, 0);
            }

            Agents[username].Balances[balanceName] += points;

            return true;
        }

        public Agent GetAgentByUsername(string username) => Agents.ContainsKey(username) ? Agents[username] : default;
    }
}
