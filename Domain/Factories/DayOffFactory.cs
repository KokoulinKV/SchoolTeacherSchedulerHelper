using Domain.Dtos;

namespace Domain.Factories
{
    /// <summary>
    /// Фабрика сущностей праздничных-выходных в РФ
    /// </summary>
    public static class DayOffFactory
    {
        /// <summary>
        /// Список праздничных-выходных дат в РФ
        /// </summary>
        private static List<DateOnly> SystemDaysOffDates = new();

        /// <summary>
        /// Метод генерации сущностей праздничных-выходных в РФ
        /// </summary>
        /// <returns></returns>
        public static List<DayOff> GenerateDaysOffEntities()
        {
            GenerateDaysOffDates();

            var systemDaysOffEntities = new List<DayOff>();

            foreach (var date in SystemDaysOffDates)
            {
                var dto = new DayOffDto
                {
                    Date = date,
                    CreatedBySystem = true
                };

                systemDaysOffEntities.Add(new DayOff(dto));
            }

            return systemDaysOffEntities;
        }

        /// <summary>
        /// Метод генерации дат праздничных-выходных в РФ на текущий и будущий год
        /// </summary>
        private static void GenerateDaysOffDates()
        {
            var currentYear = DateTime.Now.Year;
            SystemDaysOffDates = new List<DateOnly>
            {
                new DateOnly(currentYear, 1, 1),
                new DateOnly(currentYear, 1, 2),
                new DateOnly(currentYear, 1, 3),
                new DateOnly(currentYear, 1, 4),
                new DateOnly(currentYear, 1, 5),
                new DateOnly(currentYear, 1, 6),
                new DateOnly(currentYear, 1, 7),
                new DateOnly(currentYear, 1, 8),
                new DateOnly(currentYear, 2, 23),
                new DateOnly(currentYear, 3, 8),
                new DateOnly(currentYear, 5, 1),
                new DateOnly(currentYear, 5, 9),
                new DateOnly(currentYear, 6, 12),
                new DateOnly(currentYear, 11, 4),
                new DateOnly(currentYear + 1, 1, 1),
                new DateOnly(currentYear + 1, 1, 2),
                new DateOnly(currentYear + 1, 1, 3),
                new DateOnly(currentYear + 1, 1, 4),
                new DateOnly(currentYear + 1, 1, 5),
                new DateOnly(currentYear + 1, 1, 6),
                new DateOnly(currentYear + 1, 1, 7),
                new DateOnly(currentYear + 1, 1, 8),
                new DateOnly(currentYear + 1, 2, 23),
                new DateOnly(currentYear + 1, 3, 8),
                new DateOnly(currentYear + 1, 5, 1),
                new DateOnly(currentYear + 1, 5, 9),
                new DateOnly(currentYear + 1, 6, 12),
                new DateOnly(currentYear + 1, 11, 4)
            };
        }
    }
}