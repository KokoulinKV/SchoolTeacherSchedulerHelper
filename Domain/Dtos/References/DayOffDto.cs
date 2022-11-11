using System.Globalization;

namespace Domain.Dtos
{
    /// <summary>
    /// DTO сущности праздничного-выходного дня
    /// </summary>
    public class DayOffDto
    {
        /// <summary>
        /// Id записи
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата праздничного-выходного дня
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Дата праздничного-выходного дня в строковом формате
        /// </summary>
        public string DateString { get => Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture); }

        /// <summary>
        /// Год праздничного-выходного дня
        /// </summary>
        public int Year { get => Date.Year; }

        /// <summary>
        /// Признак создания праздничного-выходного дня системой
        /// </summary>
        public bool CreatedBySystem { get; set; }
    }
}