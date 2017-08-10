using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueWarriors.Mvc.Helpers;
using BlueWarriors.Mvc.ViewModels;
using BlueWarriors.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlueWarriors.Mvc.Controllers
{
    public class AgentsController : Controller
    {
        private readonly IAgent _agentService;
        private readonly ILogger<AgentsController> _logger;
        private readonly ISmsMessage _smsService;

        public AgentsController(IAgent agentService, ILogger<AgentsController> logger, ISmsMessage smsService)
        {
            _agentService = agentService;
            _logger = logger;
            _smsService = smsService;
        }

        public IActionResult Index(string search)
        {
            if(string.IsNullOrWhiteSpace(search))
            {
                ViewData["Message"] = "Enter name or MSISDN to search for agent";
                return View(null);
            }

            var model = _agentService.AgentsList().Where(a => 
                a.Name.ToLower().Contains(search.ToLower()) || 
                a.Msisdn.ToString().Contains(search))
                .Select(a => new AgentViewModel{
                    AgentId = a.AgentId,
                    Msisdn = a.Msisdn,
                    Status = a.Status,
                    Name = a.Name
                });

            if(model != null)
            {
                ViewData["Message"] = $"Returned {model.Count()} agents from the search";
                return View(model.ToList());
            }
            
            ViewData["Message"] = "Enter name or MSISDN to search for agent";
            return View(null);
        }

        public IActionResult File(int? id)
        {
            if(id == null)
            {
                ViewData["Message"] = "Please select agent to view file";
                return View("index");
            }

            int agentId;
            if(int.TryParse(id.ToString(), out agentId))
            {
                var agentQuery = _agentService.GetAgent(agentId);
                if(agentQuery == null)
                {
                    ViewData["Message"] = "Agent file doesn't exist or was not created";
                    return View("index");
                }

                var model = new AgentViewModel{
                    AgentId = agentQuery.AgentId,
                    AgentTypeId = agentQuery.AgentTypeId,
                    AuthPassword = agentQuery.AuthPassword,
                    LeaderId = agentQuery.LeaderId,
                    Msisdn = agentQuery.Msisdn,
                    Name = agentQuery.Name,
                    RowVersion = agentQuery.RowVersion,
                    Status = agentQuery.Status
                };

                return View(model);
            }

            ViewData["Message"] = "Agent file doesn't exist or was not created";
            return View("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult File(AgentViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = _agentService.UpdateAgent(new Agent{
                    AgentId = model.AgentId,
                    Name = model.Name,
                    Msisdn = model.Msisdn,
                    LeaderId = model.LeaderId,
                    AgentTypeId = model.AgentTypeId,
                    Status = model.Status,
                    RowVersion = model.RowVersion,
                    AuthPassword = model.AuthPassword
                });
                
                if(model.AuthPassword != string.Empty && !string.IsNullOrWhiteSpace(model.AuthPassword))
                {
                    _smsService.Send($"Your SIM registration app password has been changed to {model.AuthPassword}", model.Msisdn.ToString());
                }

                return RedirectToAction("file", routeValues: new{ id = result});
            }
            return View(model);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AgentViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = _agentService.AddAgent(new Agent{
                    Name = model.Name,
                    Msisdn = model.Msisdn,
                    AuthPassword = model.AuthPassword
                });                
                
                if(model.AuthPassword != string.Empty && !string.IsNullOrWhiteSpace(model.AuthPassword))
                {
                    _smsService.Send($"Your SIM registration app password is {model.AuthPassword}", model.Msisdn.ToString());
                }

                return RedirectToAction("file", routeValues: new{id = result});
            }

            return View(model);
        }

        public IActionResult Deactivated(string search, int? page)
        {
            var deactivatedAgents = _agentService.DeactivatedAgentsList();

            var model = new List<AgentViewModel>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                page = 1;

                model = deactivatedAgents.Where(a => 
                        a.Name.ToUpper().Contains(search.ToUpper()) || 
                        a.Msisdn.ToString().Contains(search))
                    .Select(a => new AgentViewModel{
                        AgentId = a.AgentId,
                        Msisdn = a.Msisdn,
                        Status = a.Status,
                        Name = a.Name
                    })
                    .ToList();
            }
            else
            {
                model = deactivatedAgents
                        .Select(a => new AgentViewModel{
                            AgentId = a.AgentId,
                            Msisdn = a.Msisdn,
                            Status = a.Status,
                            Name = a.Name
                        })
                        .ToList();
            }

            int pageSize = 10;
            return View(PaginatedList<AgentViewModel>.Create(model, page ?? 1, pageSize));
        }

        public IActionResult Deactivate(int? id)
        {
            if (id != null)
            {
                int agentId;
                if (int.TryParse(id.ToString(), out agentId))
                {
                    _agentService.DeactivateAgent(agentId);
                    ViewData["Message"] = "Agent deactivated successfully";
                    return View("index");
                }
            }

            return View("error");
        }
    }
}