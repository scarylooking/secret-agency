using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SecretAgency.Models;
using SecretAgency.Services.Interfaces;

namespace SecretAgency.Services
{
    public class AgentService : IAgentService
    {
        private readonly IMongoCollection<Agent> _agents;

        private readonly IConfiguration _configuration;
        private readonly ILogger<AgentService> _logger;


        public AgentService(ILogger<AgentService> logger, IConfiguration configuration, IMongoConnectionService mongoConnectionService)
        {
            _logger = logger;
            _configuration = configuration;

            _agents = mongoConnectionService.GetAgentCollection();
        }

        public async Task<bool> CreateNewAgent(Agent newAgent)
        {
            if (await AgentExists(newAgent.Username)) return false;

            await _agents.InsertOneAsync(newAgent);

            return true;
        }

        public async Task<bool> AddPointsToAgent(string username, string balanceName, int points)
        {
            if (!await AgentExists(username)) return false;

            var agentRecord = await GetAgentByUsername(username);

            if (!agentRecord.Balances.ContainsKey(balanceName))
            {
                agentRecord.Balances.Add(balanceName, 0);
            }

            agentRecord.Balances[balanceName] += points;

            await _agents.ReplaceOneAsync(a => a.Username == username, agentRecord);

            return true;
        }

        public async Task<Agent> GetAgentByUsername(string username)
        {
            if (!await AgentExists(username)) return default;

            var result = await _agents.FindAsync(a => a.Username == username);

            return await result.SingleOrDefaultAsync();
        }

        public async Task<bool> AgentExists(string username)
        {
            var count = await _agents.FindAsync(a => a.Username == username);

            return await count.AnyAsync();
        }
    }
}
