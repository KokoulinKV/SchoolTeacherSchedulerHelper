using System.Globalization;

namespace Domain.Dtos
{
    /// <summary>
    /// DTO сущности Выходной
    /// </summary>
    public class DayOffDto
    {
        /// <summary>
        /// Id записи
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата выходного
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Дата выходного
        /// </summary>
        public string DateString { get => Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture); }

        /// <summary>
        /// Год выходного
        /// </summary>
        public int Year { get => Date.Year; }

        /// <summary>
        /// Выходной по календарю, а не добавленный из-за переноса
        /// </summary>
        public bool CreatedBySystem { get; set; }
    }
}