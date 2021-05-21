using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Models
{
    /// <summary>
    /// Родительский класс для объектов, привязанных к региону
    /// </summary>
    public class RegionConnectedObject : AbstractEntity
    {
        /// <summary>
        /// Идентификатор региона
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Объект региона
        /// </summary>
        public virtual Region Region { get; set; }
    }
}
