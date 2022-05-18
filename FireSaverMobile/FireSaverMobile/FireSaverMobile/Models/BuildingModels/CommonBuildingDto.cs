using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models.BuildingModels
{
    public class CommonBuildingDto
    {
        public int Id { get; set; }
        public IList<UserInfoDto> ResponsibleUsers { get; set; }
        public string Address { get; set; }
        public string Info { get; set; }
        public Position BuildingCenterPosition { get; set; }
        public double? SafetyDistance { get; set; }
        public IList<ShelterDto> Shelters { get; set; }
    }
}
