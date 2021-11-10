using System.Threading.Tasks;
using SecretAgency.Models;

namespace SecretAgency.Services.Interfaces
{
    public interface IAgentService
    {
        Task<bool> AgentExists(string username);
        Task<Agent> GetAgentByUsername(string username);
        Task<bool> CreateNewAgent(Agent newAgent);
        Task<bool> AddPointsToAgent(string username, string balanceName, int points);
    }
}