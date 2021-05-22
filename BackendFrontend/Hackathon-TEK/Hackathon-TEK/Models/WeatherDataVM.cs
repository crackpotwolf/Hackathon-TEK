using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Models
{
    public class WeatherDataVM
    {
        public string station_name { get; set; }

        public string station_region { get; set; }

        public List<double> station_coordinate { get; set; }


    }
}
