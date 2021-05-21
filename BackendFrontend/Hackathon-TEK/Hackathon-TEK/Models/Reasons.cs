using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Models
{
    /// <summary>
    /// Причины возникновения ЧС
    /// </summary>
    public class Reason : RegionConnectedObject
    {
        public DateTime Date { get; set; }

        /// <summary>
        /// Тип индустрии
        /// </summary>
        public string IndustryType { get; set; }

        /// <summary>
        /// Тип события
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public string ReasonDescription { get; set; }
    }
}
