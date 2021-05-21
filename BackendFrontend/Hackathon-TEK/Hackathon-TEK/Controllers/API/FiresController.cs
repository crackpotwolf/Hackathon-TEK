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
    public class FiresController : ControllerBase
    {
        protected readonly IRepository<Fire> _repository;
        private readonly ILogger<IndexModel> _logger;

        public FiresController(ILogger<IndexModel> logger, IRepository<Fire> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Получение списка пожаров
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
                return Ok(_repository.GetListQuery().Include(p=>p.Reason));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");

                return StatusCode(500, $"Не удалось ...");
            }
        }

        /// <summary>
        /// Запись списка пожаров
        /// </summary>
        /// <returns></returns>
        [HttpPost("Post")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Post(List<Fire> data)
        {
            try
            {
                var regions = _repository.GetList().Join(
                    data,
                    p => new { p.Latitude, p.Longitude, p.AcqDate, p.AcqTime, p.RegionId },
                    d => new { d.Latitude, d.Longitude, d.AcqDate, d.AcqTime, d.RegionId },
                    (p, d) =>
                    {
                        d.Id = p.Id;
                        return d;
                    })
                    .ToList();
                _repository.AddRange(data.ExceptBy(regions, p => new { p.Latitude, p.Longitude, p.AcqDate, p.AcqTime, p.RegionId }));

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
