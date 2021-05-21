using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Models
{
    public class Earthquake : RegionConnectedObject
    {
        public int? ReasonId { get; set; }

        public virtual Reason Reason { get; set; }

        public string id { get; set; }

        public double Magnitude { get; set; }

        public string Title { get; set; }

        public DateTime Update { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public int Elevation { get; set; }

        public string Link { get; set; }

    }
}
