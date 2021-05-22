using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Hackathon_TEK.ModelsView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackathon_TEK.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Region> _regionsRepos;
        private readonly IRepository<Weather> _weatherRepository;
        private readonly ILogger<IndexModel> _logger;

        public HomeController(IRepository<Region> regionsRepos, IRepository<Weather> weatherRepository, ILogger<IndexModel> logger)
        {
            this._regionsRepos = regionsRepos;
            _logger = logger;
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                return new Dictionary<string, int>();
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
                    Temperature = weather.TempAverage >= 0 ? $"+{weather.TempAverage}" : $"{weather.TempAverage}",
                    WindSpeed = $"{weather.WindSpeedMax} м/с",
                    Description = weather.CloudsMax.ToString(),
                    Humidity = $"{weather.HumidityMax} %",
                    Fires = "",
                    Earthquake = "",
                    ProbabilityEmergency = "",
                };

                return PartialView("_RegionPartial", regionInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                return BadRequest();
            }
        }

        public IActionResult GetRegionsWithProbably(string date) 
        {
            try
            {
                List<RegionsInfo> regionsInfo = new List<RegionsInfo>()
                {
                    {new RegionsInfo {Name = "Амурская область", Probably = "80"} },
                    {new RegionsInfo {Name = "Амурская область", Probably = "80"} },
                    {new RegionsInfo {Name = "Амурская область", Probably = "80"} },
                    {new RegionsInfo {Name = "Амурская область", Probably = "80"} },
                    {new RegionsInfo {Name = "Амурская область", Probably = "80"} },
                    {new RegionsInfo {Name = "Амурская область", Probably = "80"} },
                    {new RegionsInfo {Name = "Амурская область", Probably = "80"} },
                    {new RegionsInfo {Name = "Амурская область", Probably = "80"} },
                    {new RegionsInfo {Name = "Амурская область", Probably = "80"} },
                    {new RegionsInfo {Name = "Алтайский край", Probably = "30" } }
                };

                return PartialView("_RegionsPartial", regionsInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                return BadRequest();
            }
        }
    }
}