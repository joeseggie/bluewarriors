using System.Collections.Generic;

namespace BlueWarriors.Services
{
    public interface IAgentLeader
    {
        List<AgentLeader> AgentLeadersList();
        AgentLeader AgentLeader(int agentLeaderId);
    }   
}