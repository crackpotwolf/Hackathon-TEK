using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hackathon_TEK.Interfaces
{
    /// <summary>
    /// Базовый интерфейс
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        int Id { get; set; }

        /// <summary>
        /// Флаг удаления
        /// </summary>
        [JsonIgnore]
        bool IsDelete { get; set; }
    }
}