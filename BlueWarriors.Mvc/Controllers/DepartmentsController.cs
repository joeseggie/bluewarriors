using System.Linq;
using System.Threading.Tasks;
using Bluewarriors.Mvc.Models;
using Bluewarriors.Mvc.Repository;
using Bluewarriors.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlueWarriors.Mvc.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IRepository<Department> _repository;

        public DepartmentsController(IRepository<Department> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var departmentsQuery = await _repository.GetAsync();
            var model = departmentsQuery.Select(d => new DepartmentViewModel{
                DepartmentId = d.DepartmentId,
                Name = d.Name
            });

            return View(model);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(DepartmentViewModel formData)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(new Department{
                    Name = formData.Name
                });

                return RedirectToAction("index");
            }

            return View(formData);
        }

        public async Task<IActionResult> Department(int id)
        {
            var departmentQuery = await _repository.GetAsync(id);
            if(departmentQuery == null){ return View("index"); }

            var model = new DepartmentViewModel{
                DepartmentId = departmentQuery.DepartmentId,
                Name = departmentQuery.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Department(DepartmentViewModel formData)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(new Department{
                    DepartmentId = formData.DepartmentId,
                    Name = formData.Name
                });

                return RedirectToAction("department", routeValues: new { id = formData.DepartmentId } );
            }

            return View(formData);
        }
    }
}