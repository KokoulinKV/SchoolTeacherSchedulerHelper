using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class MappingEntitiesToDtos
    {
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