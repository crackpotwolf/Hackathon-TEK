using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hackathon_TEK.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public virtual IActionResult HomeView()
        {
            return PartialView("_HomePartial");
        }

        public Dictionary<string, int> GetRegions(string date)
        {
            try
            {
                return new Dictionary<string, int>()
                {
                    {"RU-KAM", 0},
                    {"RU-BRY", 1},
                    {"RU-ALT", 3},
                    {"RU-VGG", 6}
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}