using System.Collections.Generic;
using FireSaverApi.DataContext;

namespace FireSaverApi.Dtos.BuildingDtos
{
    public class BuildingInfoDto
    {
        public int Id { get; set; }
        public IList<UserInfoDto> ResponsibleUsers { get; set; }
        public string Address { get; set; }
        public string Info { get; set; }
        public string BuildingCenterPosition { get; set; }
        public double? SafetyDistance { get; set; }
    }
}