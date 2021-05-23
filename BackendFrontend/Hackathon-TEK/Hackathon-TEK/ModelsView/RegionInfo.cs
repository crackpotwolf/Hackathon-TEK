using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.ModelsView
{
    public class RegionInfo
    {
        public string Region { get; set; }
        public string Name { get; set; } 
        public string Temperature { get; set; }
        public string WindSpeed { get; set; }
        public string Description { get; set; }
        public string Humidity { get; set; }
        public string Fires { get; set; }
        public string Earthquake { get; set; }
        public string Damage { get; set; }
        public string ProbabilityEmergency { get; set; }

        public string Event { get; set; }

        public string ChartLabels { get; set; }
        public string ChartData { get; set; }
    }
}