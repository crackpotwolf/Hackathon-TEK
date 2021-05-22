﻿using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        protected readonly IRepository<Weather> _repository;
        protected readonly IRepository<Region> _regionRepository;
        private readonly ILogger<IndexModel> _logger;

        public WeatherController(ILogger<IndexModel> logger, IRepository<Weather> repository, IRepository<Region> regionRepository)
        {
            _logger = logger;
            _repository = repository;
            _regionRepository = regionRepository;
        }

        /// <summary>
        /// Получение списка погоды
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repository.GetListQuery().ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");

                return StatusCode(500, $"Не удалось ...");
            }
        }

        [HttpGet("GetByDate")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult GetByDate(string regionMapId, string date)
        {
            try
            {
                var dateTime = DateTime.ParseExact(date,"dd.MM.yyyy", CultureInfo.InvariantCulture);
                return Ok(_repository.GetListQuery().FirstOrDefault(p => p.Date == dateTime && p.Region.MapId == regionMapId));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");

                return StatusCode(500, $"Не удалось ...");
            }
        }

        [HttpGet("GetByPeriod")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult GetByPeriod(string name, DateTime start, DateTime end)
        {
            try
            {
                var region = _regionRepository.GetListQuery().FirstOrDefault(p=>p.Name==name);
                return Ok(_repository.GetListQuery().Where(p => p.Date >= start && p.Date<=end && p.RegionId==region.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");

                return StatusCode(500, $"Не удалось ...");
            }
        }

        /// <summary>
        /// Запись списка данных о погоде
        /// </summary>
        /// <returns></returns>
        [HttpPost("Post")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Post(Dictionary<string, WeatherDataVM> data)
        {
            try
            {
                var results = new List<Weather>();
                foreach (var obj in data.Values) {
                    var region = _regionRepository.GetListQuery().FirstOrDefault(p => p.Name == obj.station_region);
                    foreach (var item in obj.weather_dates)
                    {
                        var res = new Weather();
                        if (region != null)
                            res.RegionId = region.Id;
                        res.Date = new DateTime(Int32.Parse(item.Key.Split(".")[2]), Int32.Parse(item.Key.Split(".")[1]), Int32.Parse(item.Key.Split(".")[0]));
                        
                        res.CloudsMax = item.Value.clouds_max;
                        res.HumidityMax = item.Value.humidity_max;
                        res.Percipitation = item.Value.precipitation_0;
                        res.PressureMax = item.Value.pressure_max;
                        res.StationLat = obj.station_coordinate[0];
                        res.StationLon = obj.station_coordinate[1];
                        res.StationName = obj.station_name;
                        res.StationRegion = obj.station_region;
                        res.TempAverage = item.Value.temp_average;
                        res.TempAverage0 = item.Value.temp_average_0;
                        res.TempDifNorm0 = item.Value.temp_dif_norm_0;
                        res.TempMax0 = item.Value.temp_max_0;
                        res.TempMin0 = item.Value.temp_min_0;
                        res.WindDegMax = item.Value.wind_deg_max;
                        res.WindSpeedMax = item.Value.wind_speed_max;
                        results.Add(res);
                }
                }
                var weather = _repository.GetList().Join(
                    results,
                    p => new { p.StationId, p.Date },
                    d => new { d.StationId, d.Date },
                    (p, d) =>
                    {
                        d.Id = p.Id;
                        return d;
                    })
                    .ToList();
                var excepted = results.ExceptBy(weather, p => new { p.StationId, p.Date }).ToList();
                _repository.AddRange(excepted);

                _repository.UpdateRange(weather);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");

                return StatusCode(500, $"Не удалось ...");
            }
        }
    }
}
