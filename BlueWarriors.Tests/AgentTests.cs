using System;
using Xunit;
using BlueWarriors.Services;
using System.Collections.Generic;

namespace BlueWarriors.Tests
{
    public class AgentTests
    {
        private readonly Agent _agentService;

        public AgentTests()
        {
            _agentService = new Agent();
        }

        [Fact]
        public void AgentIsAuthenticatedWithValidPassword()
        {
            var result = _agentService.IsAuthenticated("711187734", "password");
            Assert.True(result);
        }

        [Fact]
        [Trait("Service", "Agent")]
        public void ReturnsListOfDeactivatedAgents()
        {
            //Given
            
            //When
            var result = _agentService.DeactivatedAgentsList();

            //Then
            Assert.IsType<List<Agent>>(result);
        }
    }
}
