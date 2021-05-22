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

        public Dictionary<string, WeatherVM> weather_dates { get; set; }
    }

    public class WeatherVM 
    { 
        public double? temp_min_0 { get; set; }

        public double? temp_average_0 { get; set; }

        public double? temp_max_0 { get; set; }

        public double? temp_dif_norm_0 { get; set; }

        public double? precipitation_0 { get; set; }

        public double? temp_average { get; set; }

        public double? pressure_max { get; set; }

        public double? humidity_max { get; set; }

        public double? wind_speed_max { get; set; }

        public double? wind_deg_max { get; set; }

        public double? clouds_max { get; set; }
    }
}
