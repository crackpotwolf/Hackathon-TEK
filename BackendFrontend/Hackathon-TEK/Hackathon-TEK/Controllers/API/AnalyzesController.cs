using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzesController : ControllerBase
    {
        public readonly IRepository<Analyze> _analyzesRepository;
        public readonly ILogger<IndexModel> _logger;

        public AnalyzesController(ILogger<IndexModel> logger, IRepository<Analyze> analyzesRepository)
        {
            _logger = logger;
            _analyzesRepository = analyzesRepository;
        }

        /// <summary>
        /// Запись списка анализов
        /// </summary>
        /// <returns></returns>
        [HttpPost("Post")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Post(List<Analyze> data)
        {
            try
            {
                var regions = _analyzesRepository.GetList()
                    .Join(
                    data,
                    p => new { p.Date, p.RegionId },
                    d => new { d.Date, d.RegionId },
                    (p, d) =>
                    {
                        d.Id = p.Id;
                        return d;
                    })
                    .ToList();
                _analyzesRepository.AddRange(data.ExceptBy(regions, p => new { p.Date, p.RegionId }).ToList());

                _analyzesRepository.UpdateRange(regions);
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
