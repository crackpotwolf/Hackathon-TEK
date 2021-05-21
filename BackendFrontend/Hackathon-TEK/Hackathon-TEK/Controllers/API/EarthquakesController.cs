using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using MoreLinq;

namespace Hackathon_TEK.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EarthquakesController : ControllerBase
    {
        protected readonly IRepository<Earthquake> _repository;
        private readonly ILogger<IndexModel> _logger;

        public EarthquakesController(ILogger<IndexModel> logger, IRepository<Earthquake> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Получение списка землетрясений
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
                return Ok(_repository.GetListQuery().Include(p => p.Reason));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");

                return StatusCode(500, $"Не удалось ...");
            }
        }

        /// <summary>
        /// Запись списка землетрясений
        /// </summary>
        /// <returns></returns>
        [HttpPost("Post")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Post(List<Earthquake> data)
        {
            try
            {
                var regions = _repository.GetList().Join(
                    data,
                    p => new { p.Lat, p.Lon, p.Update, p.RegionId },
                    d => new { d.Lat, d.Lon, d.Update, d.RegionId },
                    (p, d) =>
                    {
                        d.Id = p.Id;
                        return d;
                    })
                    .ToList();
                _repository.AddRange(data.ExceptBy(regions, p => new { p.Lat, p.Lon, p.Update, p.RegionId }));

                _repository.UpdateRange(regions);
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
