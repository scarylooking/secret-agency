using SecretAgency.Models;

namespace SecretAgency.Services
{
    public interface IAgentDataService
    {
        bool AddPointsToAgent(string username, string balanceName, int points);
        Agent GetAgentByUsername(string username);
        bool CreateNewAgent(Agent newAgent);
    }
}