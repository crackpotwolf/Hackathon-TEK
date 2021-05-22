using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Models
{
    public class Analyze : RegionConnectedObject
    {
        public DateTime Date { get; set; }

        public double Probability { get; set; }

        public string ObjectType { get; set; }

        public string EventType { get; set; }
    }
}
