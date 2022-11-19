using Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// Класс сущности Выходной
    /// </summary>
    public class DayOff
    {
        /// <summary>
        /// Конструктор сущности Выходной
        /// </summary>
        /// <param name="date"></param>
        public DayOff(DayOffDto dayOffDto)
        {
            this.Id = Guid.NewGuid();
            this.Date = dayOffDto.Date;
            this.CreatedBySystem = dayOffDto.CreatedBySystem;
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
        /// Выходной по календарю, а не добавленный из-за переноса
        /// </summary>
        public bool CreatedBySystem { get; private set; }
    }
}