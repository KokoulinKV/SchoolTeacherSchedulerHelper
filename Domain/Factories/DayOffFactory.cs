using Domain.Dtos;
using Domain.Entities.References;

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
        /// Метод генерации дат праздничных-выходных в РФ на прошлый, текущий и будущий календарный год
        /// </summary>
        private static void GenerateDaysOffDates()
        {
            var currentYear = DateTime.Now.Year;
            SystemDaysOffDates.AddRange(GenerateDaysOffDatesForSpecialYear(currentYear - 1));
            SystemDaysOffDates.AddRange(GenerateDaysOffDatesForSpecialYear(currentYear));
            SystemDaysOffDates.AddRange(GenerateDaysOffDatesForSpecialYear(currentYear + 1));
        }

        /// <summary>
        /// Метод генерации дат праздничных-выходных в РФ на конкретный календарный год
        /// </summary>
        private static List<DateOnly> GenerateDaysOffDatesForSpecialYear(int year)
        {
            return new List<DateOnly>
            {
                new DateOnly(year, 1, 1),
                new DateOnly(year, 1, 2),
                new DateOnly(year, 1, 3),
                new DateOnly(year, 1, 4),
                new DateOnly(year, 1, 5),
                new DateOnly(year, 1, 6),
                new DateOnly(year, 1, 7),
                new DateOnly(year, 1, 8),
                new DateOnly(year, 2, 23),
                new DateOnly(year, 3, 8),
                new DateOnly(year, 5, 1),
                new DateOnly(year, 5, 9),
                new DateOnly(year, 6, 12),
                new DateOnly(year, 11, 4)
            };
        }
    }
}