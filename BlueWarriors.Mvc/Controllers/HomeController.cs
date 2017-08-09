using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueWarriors.Mvc.ViewModels;
using BlueWarriors.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlueWarriors.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAgent _agentService;

        public HomeController(IAgent agentService)
        {
            _agentService = agentService;
        }
        public IActionResult Index()
        {
            // var model = _agentService.AgentsList();
            // if(model == null)
            // {
            //     return View(null);
            // }
            // return View(model.ToList());
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AgentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var agentId = _agentService.AddAgent(new Agent{
                    Name = model.Name,
                    Msisdn = model.Msisdn,
                    AuthPassword = model.AuthPassword
                });

                return RedirectToAction("agent", routeValues: new {id = agentId});
            }

            return View(model);
        }

        public IActionResult Agent(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("index");
            }

            int agentId;
            if (int.TryParse(id, out agentId))
            {
                var model = _agentService.GetAgent(agentId);
                return View(model);
            }

            return RedirectToAction("index");
        }

        [HttpPost]
        public IActionResult Agent(AgentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var agentId = _agentService.UpdateAgent(new Agent{
                    AgentId = model.AgentId,
                    Name = model.Name,
                    Msisdn = model.Msisdn,
                    LeaderId = model.LeaderId,
                    AgentTypeId = model.AgentTypeId,
                    Status = model.Status,
                    RowVersion = model.RowVersion,
                    AuthPassword = model.AuthPassword
                });
                return RedirectToAction("agent", routeValues: new {id = agentId});
            }
            
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
