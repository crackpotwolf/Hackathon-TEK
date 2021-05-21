using Hackathon_TEK.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NIOKR_Main.Models
{
    /// <summary>
    /// Базовый класс
    /// </summary>
    public abstract class AbstractEntity : IEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Флаг удаления
        /// </summary>
        [JsonIgnore]
        public bool IsDelete { get; set; } = false;
    }
}