using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BlueWarriors.Services
{
    public class AgentLeader : IAgentLeader
    {
        public List<AgentLeader> AgentLeadersList()
        {
            throw new NotImplementedException();
        }

        AgentLeader IAgentLeader.AgentLeader(int agentLeaderId)
        {
            throw new NotImplementedException();
        }
        
        private SqlConnection DatabaseConnection()
        {
            return new SqlConnection("Data Source=172.25.0.26;Initial Catalog=BW;User Id=pps;Password=pps1234;");
        }
    }
}