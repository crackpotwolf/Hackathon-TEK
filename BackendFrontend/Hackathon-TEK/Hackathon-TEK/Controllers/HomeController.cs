using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Hackathon_TEK.ModelsView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<Fire> _fireRepository;
        private readonly IRepository<Earthquake> _earthRepository;
        private readonly IRepository<Analyze> _analyzeRepository;
        private readonly IRepository<Reason> _reasonsRepository;
        public HomeController(IRepository<Region> regionsRepos,
            IRepository<Weather> weatherRepository,
            IRepository<Fire> fireRepository,
            IRepository<Earthquake> earthRepository,
            IRepository<Reason> reasonsRepository,
            IRepository<Analyze> analyzeRepository,
            ILogger<IndexModel> logger)
        {
            this._regionsRepos = regionsRepos;
            _logger = logger;
            _weatherRepository = weatherRepository;
            _fireRepository = fireRepository;
            _earthRepository = earthRepository;
            _reasonsRepository = reasonsRepository;
            _analyzeRepository = analyzeRepository;
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
                //var regions = _regionsRepos.GetListQuery().Select(p => new KeyValuePair<string, int>(p.MapId,new Random().Next(0,6)));
                var analyzes = _analyzeRepository.GetListQuery()
                    .Where(p => p.Date.Date == DateTime.ParseExact(date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture)).Include(p=>p.Region).ToList()
                    .Select(p => new KeyValuePair<string, int>(p.Region.MapId, CalculateVariance(p.Probability))).ToDictionary(p => p.Key, p => p.Value);
                return analyzes;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                return new Dictionary<string, int>();
            }
        }

        public IActionResult GetProbabilities(string date, string region)
        {
            try
            {
                var dateTime = DateTime.ParseExact(date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var regionObj = _regionsRepos.GetListQuery().First(p => p.MapId == region);
                var data = _analyzeRepository.GetListQuery()
                    .Where(p => p.RegionId == regionObj.Id && p.Date.Date >= dateTime.AddDays(-3) && p.Date.Date <= dateTime.AddDays(3))
                    .OrderBy(p => p.Date)
                    .Select(p => new KeyValuePair<string, double>(p.Date.ToString("dd.MM.yyyy"), Math.Round(p.Probability * 100, 2))).ToDictionary(p=>p.Key,p=>p.Value);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        public int CalculateVariance(double probability)
        {
            if (probability <= 0.2)
                return 0;
            else if (probability <= 0.4)
                return 1;
            else if (probability <= 0.6)
                return 2;
            else if (probability <= 0.8)
                return 3;
            else return 4;
        }

        public IActionResult GetRegion(string region, string region_name, string date) 
        {
            try
            {
                var regionObj = _regionsRepos.GetListQuery().First(p => p.MapId == region);
                var dateTime = DateTime.ParseExact(date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Weather weather = null;

                if(_weatherRepository.GetListQuery().Any(p => p.Date.Date == dateTime && p.RegionId == regionObj.Id))
                weather = _weatherRepository.GetListQuery()
                    .First(p => p.Date.Date == dateTime
                        && p.RegionId==regionObj.Id);

                Fire fire = null;

                if(_fireRepository.GetListQuery()
                    .Any(p => p.Date.Date == dateTime && p.RegionId == regionObj.Id))
                        fire = _fireRepository.GetListQuery()
                            .Where(p => p.Date.Date == dateTime && p.RegionId == regionObj.Id)
                            .OrderByDescending(p=>p.Confidence).First();

                Earthquake earth = null;

                if (_earthRepository.GetListQuery()
                    .Any(p => p.Update.Date == dateTime && p.RegionId == regionObj.Id))
                    earth = _earthRepository.GetListQuery()
                        .Where(p => p.Update.Date == dateTime && p.RegionId == regionObj.Id)
                        .OrderByDescending(p => p.Magnitude).First();

                Analyze analyze = null;
                if (_analyzeRepository.GetListQuery().Any(p => p.Date.Date == dateTime && p.RegionId == regionObj.Id))
                    analyze = _analyzeRepository.GetListQuery().First(p => p.Date.Date == dateTime && p.RegionId == regionObj.Id);

                RegionInfo regionInfo = new RegionInfo()
                {
                    Region = region,
                    Name = region_name,
                    Temperature = weather!=null ? weather.TempAverage >= 0 ? $"+{weather.TempAverage}" : $"{weather.TempAverage}" : "-",
                    WindSpeed = weather != null ? $"{weather.WindSpeedMax} м/с" : "-",
                    Description = weather != null ? weather.CloudsMax.ToString() : "-",
                    Humidity = weather != null ? $"{weather.HumidityMax} %" : "-",
                    Fires = fire!=null ? $"{fire.Confidence} %" : "-",
                    Earthquake = earth!=null ? earth.Magnitude.ToString() : "-",
                    ProbabilityEmergency = analyze!=null ? $"{Math.Round(analyze.Probability*100, 2)} %" : "-",
                    Damage = analyze != null ? $"{analyze.ObjectType}" : "-",
                    Event= analyze != null ? $"{analyze.EventType}" : "-"
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
                List<RegionsInfo> regionsInfo = new List<RegionsInfo>();

                var regions = _regionsRepos.GetListQuery().ToList();

                foreach (var region in regions)
                {
                    regionsInfo.Add(new RegionsInfo 
                    {
                        Name = region.Name,
                        MapId = region.MapId,
                        Probably = new Random().Next(0, 100).ToString(),
                        EventType = "",
                        ObjectType = ""
                    });
                }
                
                var analyzeData = _analyzeRepository.GetListQuery()
                    .Where(p => p.Date.Date == DateTime.ParseExact(date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture))
                    .Include(p => p.Region)
                    .ToList();

                return PartialView("_RegionsPartial", analyzeData
                    .Select(p => new RegionsInfo() { MapId = p.Region.MapId, Name = p.Region.Name, Probably = (Math.Round(p.Probability * 100, 2)).ToString(), EventType = p.EventType, ObjectType = p.ObjectType }));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                return BadRequest();
            }
        }

        public virtual IActionResult GetFeedback() 
        {
            // Cписок типов событий
            var eventTypes = _reasonsRepository.GetListQuery().Select(p => p.EventType).Distinct().ToList();
            ViewBag.eventTypes = eventTypes;

            // Cписок типов объектов
            var objectTypes = _reasonsRepository.GetListQuery().Select(p => p.TypeObject).Distinct().ToList();
            ViewBag.objectTypes = objectTypes;

            return PartialView("_FeedbackPartial");
        }

        public virtual void PostFeedback(string date, string eventTypes, string objectTypes, string details, string region) 
        {
            try
            {
                var reason = new Reason();
                reason.Date = DateTime.ParseExact(date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                reason.TypeObject = objectTypes;
                reason.EventType = eventTypes;
                reason.ReasonDescription = details;

                var regionObj = _regionsRepos.GetListQuery().First(p => p.MapId == region);

                reason.RegionId = regionObj.Id;
                _reasonsRepository.Add(reason);
            }
            catch(Exception ex)
            {

            }
        }
    }
}