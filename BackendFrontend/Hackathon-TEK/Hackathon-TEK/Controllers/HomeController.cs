using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackathon_TEK.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Region> _regionsRepos;

        public HomeController(IRepository<Region> regionsRepos)
        {
            this._regionsRepos = regionsRepos;
        }

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
                var regions = _regionsRepos.GetListQuery().Select(p => new KeyValuePair<string, int>(p.MapId,new Random().Next(0,6)));
                return regions.ToDictionary(p=>p.Key, p=>p.Value);
                //return new Dictionary<string, int>()
                //{
                //    {"RU-KAM", 0},
                //    {"RU-BRY", 1},
                //    {"RU-ALT", 3},
                //    {"RU-VGG", 6}
                //};
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}