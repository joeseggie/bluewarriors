using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueWarriors.Mvc.ViewModels;
using BlueWarriors.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlueWarriors.Mvc.Controllers
{
    public class RegistrationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}