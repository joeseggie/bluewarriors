using System.Linq;
using System.Threading.Tasks;
using Bluewarriors.Mvc.Models;
using Bluewarriors.Mvc.Repository;
using BlueWarriors.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlueWarriors.Mvc.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IRepository<Region> _repository;

        public RegionsController(IRepository<Region> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var regionsQuery = await _repository.GetAsync();
            var model = regionsQuery
                .Select(r => new RegionViewModel{
                    RegionId = r.RegionId,
                    Name = r.Name
                }).ToList();

            return View(model);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(RegionViewModel formData)
        {
            if(ModelState.IsValid)
            {
                await _repository.AddAsync(new Region{
                    Name = formData.Name
                });
                return RedirectToAction("index");
            }

            return View(formData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Region(int id)
        {
            var regionQuery = await _repository.GetAsync(id);
            if(regionQuery == null){ return View("index"); }

            var model = new RegionViewModel{
                RegionId = regionQuery.RegionId,
                Name = regionQuery.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Region(RegionViewModel formData)
        {
            if(ModelState.IsValid)
            {
                await _repository.UpdateAsync(new Region{
                    RegionId = formData.RegionId,
                    Name = formData.Name
                });

                return RedirectToAction("region", routeValues: new { id = formData.RegionId } );
            }

            return View(formData);
        }
    }
}