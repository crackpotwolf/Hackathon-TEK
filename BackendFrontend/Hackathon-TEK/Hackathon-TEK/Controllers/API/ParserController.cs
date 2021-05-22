using Hackathon_TEK.Extensions;
using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hackathon_TEK.Controllers.API
{
    [Route("api/Test")]
    [ApiController]
    public class ParserController : Controller
    {
        private readonly IRepository<Reason> reasonsRepos;
        private readonly IRepository<Region> regionsRepos;
        public ParserController(IRepository<Reason> reasonsRepos,
            IRepository<Region> regionsRepos)
        {
            this.reasonsRepos = reasonsRepos;
            this.regionsRepos = regionsRepos;
        }

        [HttpGet]
        public IActionResult WeatherReasons()
        {
            var path = @"D:\Downloads\Preprocessing\Аварии_погода_САЦ.csv";
            var csv_text = new FileInfo(path).ReadAllFile();
            new Regex("[^\"\'   ]+\n");



            var entities = new Regex("[^\"\'   ]+\n").Split(csv_text)
                .Skip(1)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p =>
                {
                    var fields = p.Split(';');
                    return new
                    {
                        Date = DateTime.ParseExact(fields[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        Idustry = fields[2],
                        Type = fields[3],
                        Region = fields[4],
                        Description = fields[5]
                    };
                    //return p;
                }).ToList();
            Console.WriteLine(entities.Count);

            var test = regionsRepos.GetList().ToList();
            var regions = regionsRepos.GetList().ToList()
                .ToDictionary(p => p.Name);

            var to_db = entities.Select(p =>
            {
                return new Reason()
                {
                    Date = p.Date,
                    ReasonDescription = p.Description,
                    EventType = p.Type,
                    IndustryType = p.Idustry,
                    IsWeather = true,
                    RegionId = regions[p.Region].Id
                };
            }).ToList();

            Console.WriteLine(to_db.Count);

            //reasonsRepos.AddRange(to_db);
            return Ok(to_db);
        }
    }
}
