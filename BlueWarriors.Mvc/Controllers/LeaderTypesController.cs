using System.Linq;
using System.Threading.Tasks;
using Bluewarriors.Mvc.Models;
using Bluewarriors.Mvc.Repository;
using BlueWarriors.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlueWarriors.Mvc.Controllers
{
    public class LeaderTypesController : Controller
    {
        private readonly IRepository<LeaderType> _repository;

        public LeaderTypesController(IRepository<LeaderType> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var leaderTypesQuery = await _repository.GetAsync();
            var model = leaderTypesQuery.Select(t => new LeaderTypeViewModel{
                LeaderTypeId = t.LeaderTypeId,
                Description = t.Description
            }).ToList();

            return View(model);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(LeaderTypeViewModel formData)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(new LeaderType{
                    Description = formData.Description
                });

                return RedirectToAction("index");
            }

            return View(formData);
        }

        public async Task<IActionResult> LeaderType(int id)
        {
            var leaderTypeQuery = await _repository.GetAsync(id);
            if(leaderTypeQuery == null) { return View("index"); }

            var model = new LeaderType{
                LeaderTypeId = leaderTypeQuery.LeaderTypeId,
                Description = leaderTypeQuery.Description
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaderType(LeaderTypeViewModel formData)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(new LeaderType{
                    LeaderTypeId = formData.LeaderTypeId,
                    Description = formData.Description
                });

                return RedirectToAction("leadertype", routeValues: new { id = formData.LeaderTypeId });
            }

            return View(formData);
        }
    }
}