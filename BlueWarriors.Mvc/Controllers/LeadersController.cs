using System.Linq;
using System.Threading.Tasks;
using Bluewarriors.Mvc.Models;
using Bluewarriors.Mvc.Repository;
using BlueWarriors.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlueWarriors.Mvc.Controllers
{
    public class LeadersController : Controller
    {
        private readonly IRepository<Leader> _repository;

        public LeadersController(IRepository<Leader> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var leadersQuery = await _repository.GetAsync();
            var model = leadersQuery.Select(l => new LeaderViewModel{
                LeaderId = l.LeaderId,
                Name = l.Name,
                Msisdn = l.Msisdn,
                RegionId = l.RegionId,
                Region = l.Region,
                DepartmentId = l.DepartmentId,
                Department = l.Department,
                LeaderTypeId = l.LeaderTypeId,
                Status = l.Status,
                DeactivationDate = l.DeactivationDate
            }).ToList();

            return View(model);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(LeaderViewModel formData)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(new Leader{
                    Name = formData.Name,
                    Msisdn = formData.Msisdn,
                    RegionId = formData.RegionId,
                    DepartmentId = formData.DepartmentId,
                    LeaderTypeId = formData.LeaderTypeId
                });
            }

            return View(formData);
        }

        public async Task<IActionResult> Leader(int id)
        {
            var leaderQuery = await _repository.GetAsync(id);
            if(leaderQuery == null) { return View("index"); }

            var model = new LeaderViewModel{
                LeaderId = leaderQuery.LeaderId,
                Name = leaderQuery.Name,
                Msisdn = leaderQuery.Msisdn,
                RegionId = leaderQuery.RegionId,
                Region = leaderQuery.Region,
                DepartmentId = leaderQuery.DepartmentId,
                Department = leaderQuery.Department,
                LeaderTypeId = leaderQuery.LeaderTypeId,
                Status = leaderQuery.Status,
                DeactivationDate = leaderQuery.DeactivationDate
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Leader(LeaderViewModel formData)
        {
            if(ModelState.IsValid)
            {
                await _repository.UpdateAsync(new Leader{
                    LeaderId = formData.LeaderId,
                    Name = formData.Name,
                    Msisdn = formData.Msisdn,
                    RegionId = formData.RegionId,
                    Region = formData.Region,
                    DepartmentId = formData.DepartmentId,
                    Department = formData.Department,
                    LeaderTypeId = formData.LeaderTypeId,
                    Status = formData.Status,
                    DeactivationDate = formData.DeactivationDate
                });

                return RedirectToAction("leader", routeValues: new { id = formData.LeaderId} );
            }

            return View(formData);
        }
    }
}