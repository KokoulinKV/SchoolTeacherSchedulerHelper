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
        public DayOff(DateTime date)
        {
            this.Id = Guid.NewGuid();
            this.Date = DateOnly.FromDateTime(date);
        }

        protected DayOff()
        { }

        /// <summary>
        /// Id записи
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Дата выходного
        /// </summary>
        [Required]
        public DateOnly Date { get; set; }
    }
}