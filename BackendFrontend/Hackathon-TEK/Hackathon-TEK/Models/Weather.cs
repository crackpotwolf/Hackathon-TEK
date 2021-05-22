using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Models
{
    public class Weather : RegionConnectedObject
    {
        public int StationId { get; set; }

        public string StationName { get; set; }

        public string StationRegion { get; set; }

        public double StationLat { get; set; }

        public double StationLon { get; set; }

        public DateTime Date { get; set; }

        public double? TempMin0 { get; set; }

        public double? TempAverage0 { get; set; }

        public double? TempMax0 { get; set; }

        public double? TempDifNorm0 { get; set; }

        public double? Percipitation { get; set; }

        public double? TempAverage { get; set; }

        public double? PressureMax { get; set; }

        public double? HumidityMax { get; set; }

        public double? WindSpeedMax { get; set; }

        public double? WindDegMax { get; set; }

        public double? CloudsMax { get; set; }

    }
}
