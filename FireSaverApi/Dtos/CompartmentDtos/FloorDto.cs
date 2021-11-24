using System.Collections.Generic;

namespace FireSaverApi.Dtos.CompartmentDtos
{
    public class FloorDto : CompartmentDto
    {
        public int Level { get; set; }
        public List<RoomDto> Rooms { get; set; }
    }
}