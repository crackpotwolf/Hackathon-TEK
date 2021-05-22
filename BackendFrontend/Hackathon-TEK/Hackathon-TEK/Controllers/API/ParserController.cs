using Hackathon_TEK.Extensions;
using Hackathon_TEK.Interfaces;
using Hackathon_TEK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using MoreLinq;
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
        private readonly IRepository<Fire> fiersRepos;
        public ParserController(IRepository<Reason> reasonsRepos,
            IRepository<Region> regionsRepos,
            IRepository<Fire> fiersRepos)
        {
            this.reasonsRepos = reasonsRepos;
            this.regionsRepos = regionsRepos;
            this.fiersRepos = fiersRepos;
        }

        #region Распарсенные данные
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


        [HttpGet("WeatherReasons2020")]
        public IActionResult WeatherReasons2020()
        {
            var path = @"D:\Downloads\Preprocessing\Аварии_погода_САЦ_2020.csv";
            var csv = new FileInfo(path).ParseCSV();

            var entities = csv
                .Skip(1)
                .Select(fields =>
                {
                    return new
                    {
                        Date = DateTime.ParseExact(fields[1], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Subject = fields[2],
                        Reason = fields[3],
                        Description = fields[4],
                        TypeObject = fields[5],
                    };
                }).ToList();

            var test = regionsRepos.GetList().ToList();
            var regions = regionsRepos.GetList().ToList()
                .ToDictionary(p => p.Name);

            var to_db = entities
            .Where(p => p.Subject != "Крым")
            .Select(p =>
            {
                return new Reason()
                {
                    Date = p.Date,
                    ReasonDescription = p.Description,
                    EventType = p.Reason,
                    TypeObject = p.TypeObject,
                    IsWeather = true,
                    RegionId = regions[p.Subject].Id
                };
            }).ToList();

            reasonsRepos.AddRange(to_db);
            return Ok(to_db);
        }


        [HttpGet("AccidentReasons2020")]
        public IActionResult AccidentReasons2020()
        {
            var path = @"D:\Downloads\Preprocessing\Аварии_Причины_САЦ_2020.csv";
            var csv = new FileInfo(path).ParseCSV();

            var entities = csv
                .Skip(1)
                .Select(fields =>
                {
                    return new
                    {
                        Date = DateTime.ParseExact(fields[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        Subject = fields[2],
                        Reason = fields[3],
                    };
                }).ToList();

            var test = regionsRepos.GetList().ToList();
            var regions = regionsRepos.GetList().ToList()
                .ToDictionary(p => p.Name);

            var except = new List<string>() { "Крым", "Санкт-Петербург" };

            var to_db = entities
            .Where(p => !except.Contains(p.Subject))
            .Select(p =>
            {
                return new Reason()
                {
                    Date = p.Date,
                    //ReasonDescription = p.Description,
                    EventType = p.Reason,
                    //TypeObject = p.TypeObject,
                    IsWeather = true,
                    RegionId = regions[p.Subject].Id
                };
            }).ToList();

            reasonsRepos.AddRange(to_db);
            return Ok(to_db);
        }


        #endregion

        [HttpGet("Fiers")]
        public IActionResult Fiers()
        {
            var path = @"D:\Downloads\Preprocessing\Пожары_2020.csv";
            var csv = new FileInfo(path).ParseCSV();

            var regions = regionsRepos.GetList().ToList()
                .ToDictionary(p => p.Name);

            var entities = csv
                .Skip(1)
                .Where(p => !string.IsNullOrWhiteSpace(p.Last()))
                .Where(p => regions.ContainsKey(p[16]))
                .Select(fields =>
                {
                    return new Fire()
                    {
                        Latitude = double.Parse(fields[2], CultureInfo.InvariantCulture),
                        Longitude = double.Parse(fields[3], CultureInfo.InvariantCulture),
                        Brightness = double.Parse(fields[4], CultureInfo.InvariantCulture),
                        Scan = double.Parse(fields[5], CultureInfo.InvariantCulture),
                        Track = double.Parse(fields[6], CultureInfo.InvariantCulture),
                        Date = DateTime.ParseExact($"{fields[7]} {fields[8]}", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        //AcqTime = TimeSpan.ParseExact(fields[8], "HH:mm:ss", CultureInfo.InvariantCulture),
                        Satellite = fields[9],
                        Confidence = (int)double.Parse(fields[10], CultureInfo.InvariantCulture),
                        BrightT31 = (int)double.Parse(fields[12], CultureInfo.InvariantCulture),
                        Frp = (int)double.Parse(fields[13], CultureInfo.InvariantCulture),
                        Federal = fields[14],
                        RegionId = regions[fields[16]].Id
                    };
                }).ToList();


            var except = new List<string>() { "Крым", "Санкт-Петербург" };

            //var to_db = entities
            //.Where(p => !except.Contains(p.Subject))
            //.Select(p =>
            //{
            //    return new Reason()
            //    {
            //        Date = p.Date,
            //        //ReasonDescription = p.Description,
            //        EventType = p.Reason,
            //        //TypeObject = p.TypeObject,
            //        IsWeather = true,
            //        RegionId = regions[p.Subject].Id
            //    };
            //}).ToList();
            foreach (var batch in entities.Batch(10_000))
            {
                fiersRepos.AddRange(batch);
            }
            return Ok(entities);
        }

    }
}
