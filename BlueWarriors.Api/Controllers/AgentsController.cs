using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.IO;
using System.Threading;
using BlueWarriors.Services;

namespace BlueWarriors.Api.Controllers
{
    [Route("api/mobile/bluewarriors")]
    public class AgentsController : Controller
    {
        private readonly IAgent _agentService;

        public AgentsController(IAgent agentService)
        {
            _agentService = agentService;
        }

        //GET api/mobile/bluewarriors/login?msisdn=&password=
        [HttpGet("login")]
        public IActionResult Get([FromQuery] string msisdn, string password)
        {
            try
            {
                return Content(_agentService.IsAuthenticated(msisdn, password).ToString());
            }
            catch (System.Exception)
            {
                return StatusCode(400);
            }
        }
    }
}