using System.Collections.Generic;

namespace BlueWarriors.Services
{
    public interface IAgent
    {
         bool IsAuthenticated(string msisdn, string password);
         IEnumerable<Agent> AgentsList();
         int AddAgent(Agent newAgent);
         Agent GetAgent(int agentId);
         int UpdateAgent(Agent agentDetails);
         List<Agent> DeactivatedAgentsList();
         void DeactivateAgent(int agentId);
    }
}