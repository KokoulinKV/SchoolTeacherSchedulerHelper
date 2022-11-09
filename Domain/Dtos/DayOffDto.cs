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
    }
}