using Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.References
{
    /// <summary>
    /// Класс сущности праздничного-выходного дня
    /// </summary>
    public class DayOff
    {
        /// <summary>
        /// Конструктор сущности праздничного-выходного дня
        /// </summary>
        /// <param name="dayOffDto">Dto сущности праздничного-выходного дня</param>
        public DayOff(DayOffDto dayOffDto)
        {
            Id = Guid.NewGuid();
            Date = dayOffDto.Date;
            CreatedBySystem = dayOffDto.CreatedBySystem;
        }

        protected DayOff()
        { }

        /// <summary>
        /// Id записи
        /// </summary>
        [Required]
        public Guid Id { get; private set; }

        /// <summary>
        /// Дата выходного
        /// </summary>
        [Required]
        public DateOnly Date { get; private set; }

        /// <summary>
        /// Признак создания праздничного-выходного дня системой
        /// </summary>
        [Required]
        public bool CreatedBySystem { get; private set; }
    }
}