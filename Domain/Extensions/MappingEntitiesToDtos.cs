using Domain.Dtos;
using Domain.Entities.References;

namespace Domain.Extensions
{
    /// <summary>
    /// Класс маппинга сущностей на их DTO
    /// </summary>
    public static class MappingEntitiesToDtos
    {
        /// <summary>
        /// Маппинг сущности празничнего-выходного дня на его DTO
        /// </summary>
        /// <param name="dayOff">Сущность празничнего-выходного дня</param>
        /// <returns>DTO сущности празничнего-выходного дня</returns>
        public static DayOffDto ToDto(this DayOff dayOff)
        {
            return new DayOffDto
            {
                Id = dayOff.Id,
                Date = dayOff.Date,
                CreatedBySystem = dayOff.CreatedBySystem
            };
        }
    }
}