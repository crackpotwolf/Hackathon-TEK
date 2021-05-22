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
    public class ReasonsController : ControllerBase
    {
        protected readonly IRepository<Reason> _repository;
        private readonly ILogger<IndexModel> _logger;

        public ReasonsController(ILogger<IndexModel> logger, IRepository<Reason> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Получение списка причин
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
                return Ok(_repository.GetListQuery());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");

                return StatusCode(500, $"Не удалось ...");
            }
        }

        /// <summary>
        /// Запись списка причин
        /// </summary>
        /// <returns></returns>
        [HttpPost("Post")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Post(List<Reason> data)
        {
            try
            {
                var regions = _repository.GetList().Join(
                    data,
                    p=>new { p.Date, p.Description, p.IndustryType, p.EventType, p.RegionId, p.ReasonDescription },
                    d=>new { d.Date, d.Description, d.IndustryType, d.EventType, d.RegionId, d.ReasonDescription },
                    (p, d)=>
                    {
                        d.Id = p.Id;
                        return d;
                    })
                    .ToList();
                _repository.AddRange(data.ExceptBy(regions, p => new { p.Date, p.Description, p.IndustryType, p.EventType, p.RegionId, p.ReasonDescription }));

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
