using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Models
{
    /// <summary>
    /// Данные о пожарах
    /// </summary>
    public class Fire : RegionConnectedObject
    {
        public int? ReasonId { get; set; }

        public virtual Reason Reason { get; set; }

        [JsonProperty("LATITUDE")]
        public double Latitude { get; set; }

        [JsonProperty("LONGITUDE")]
        public double Longitude { get; set; }

        [JsonProperty("BRIGHTNESS")]
        public double Brightness { get; set; }

        [JsonProperty("SCAN")]
        public double Scan { get; set; }

        [JsonProperty("TRACK")]
        public double Track { get; set; }

        [JsonProperty("ACQ_DATE")]
        public DateTime Date { get; set; }

        [JsonProperty("SATELLITE")]
        public string Satellite { get; set; }

        [JsonProperty("CONFIDENCE")]
        public int Confidence { get; set; }

        [JsonProperty("VERSION")]
        public string Version { get; set; }

        [JsonProperty("BRIGHT_T31")]
        public double BrightT31 { get; set; }

        [JsonProperty("FRP")]
        public double Frp { get; set; }

        [JsonProperty("Федеральны")]
        public string Federal { get; set; }

        [JsonProperty("Район")]
        public string District { get; set; }

        [JsonProperty("Субъект")]
        public string Subject { get; set; }
    }
}
