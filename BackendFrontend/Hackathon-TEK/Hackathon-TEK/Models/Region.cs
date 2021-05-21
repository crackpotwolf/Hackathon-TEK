using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon_TEK.Models
{
    /// <summary>
    /// Регион (субъект федерации)
    /// </summary>
    public class Region : AbstractEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полигон
        /// </summary>
        public string Coordinates { get; set; }

        /// <summary>
        /// Ссылка на RSS лонту оперативной сводки МЧС региона
        /// </summary>
        public string RssUrl { get; set; }
    }
}
