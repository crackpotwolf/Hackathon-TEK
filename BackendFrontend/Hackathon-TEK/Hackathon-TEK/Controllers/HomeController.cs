using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Hackathon_TEK.ModelsView;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackathon_TEK.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Region> _regionsRepos;
        private readonly IRepository<Weather> _weatherRepository;

        public HomeController(IRepository<Region> regionsRepos, IRepository<Weather> weatherRepository)
        {
            this._regionsRepos = regionsRepos;
            _weatherRepository = weatherRepository;
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
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult GetRegion(string region, string region_name, string date) 
        {
            try
            {
                var regionObj = _regionsRepos.GetListQuery().First(p => p.MapId == region);

                var weather = _weatherRepository.GetListQuery()
                    .First(p => p.Date == DateTime.ParseExact(date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture)
                        && p.RegionId==regionObj.Id);

                RegionInfo regionInfo = new RegionInfo()
                {
                    Name = region_name,
                    Temperature = weather.TempAverage >= 0 ? $"+{weather.TempAverage.ToString()}" : $"{weather.TempAverage.ToString()}",
                    WindSpeed = $"{weather.WindSpeedMax.ToString()} м/с",
                    Description = weather.CloudsMax.ToString(),
                    Humidity = $"{weather.HumidityMax.ToString()} %",
                    Fires = "",
                    Earthquake = "",
                    ProbabilityEmergency = "",
                };

                return PartialView("_RegionPartial", regionInfo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}