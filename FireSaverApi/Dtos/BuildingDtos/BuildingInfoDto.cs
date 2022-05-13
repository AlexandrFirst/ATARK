using System.Collections.Generic;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;

namespace FireSaverApi.Dtos.BuildingDtos
{
    public class BuildingInfoDto
    {
        public int Id { get; set; }
        public IList<UserInfoDto> ResponsibleUsers { get; set; }
        public IList<FloorDto> Floors { get; set; }
        public string Address { get; set; }
        public string Info { get; set; }
        public PositionDto BuildingCenterPosition { get; set; }
        public IList<ShelterDto> Shelters { get; set; }
        public double? SafetyDistance { get; set; }
    }
}