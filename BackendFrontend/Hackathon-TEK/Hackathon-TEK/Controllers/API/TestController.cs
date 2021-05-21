using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using System;

namespace Hackathon_TEK.Controllers.API
{
    [Route("api/Test")]
    [ApiController]
    public class TestController : Controller
    {
        protected readonly IRepository<Test> _test;
        private readonly ILogger<IndexModel> _logger;

        public TestController(ILogger<IndexModel> logger, IRepository<Test> test) 
        {
            _logger = logger;
            _test = test;
        }

        [HttpGet("Get")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Get()
        {
            try
            {

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