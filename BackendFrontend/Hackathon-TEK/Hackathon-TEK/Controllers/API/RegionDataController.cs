using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Controllers.API
{
    [Route("api/Region")]
    [ApiController]
    public class RegionDataController : Controller
    {
        protected readonly IRepository<Region> _regionRepository;
        private readonly ILogger<IndexModel> _logger;

        public RegionDataController(ILogger<IndexModel> logger, IRepository<Region> regionRepository)
        {
            _logger = logger;
            _regionRepository = regionRepository;
        }

        /// <summary>
        /// Получение списка регионов
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
                return Ok(_regionRepository.GetList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");

                return StatusCode(500, $"Не удалось ...");
            }
        }

        /// <summary>
        /// Запись списка регионов
        /// </summary>
        /// <returns></returns>
        [HttpPost("Post")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Post(Dictionary<string, Dictionary<string, List<List<double>>>> data)
        {
            try
            {
                var regions=data.Select(p => new Region() { Name = p.Key, Coordinates = JsonConvert.SerializeObject(p.Value) });
                
                _regionRepository.AddRange(regions);
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
