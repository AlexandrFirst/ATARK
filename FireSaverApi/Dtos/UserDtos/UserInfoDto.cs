using System;
using System.Collections.Generic;
using FireSaverApi.Dtos.BuildingDtos;

namespace FireSaverApi.Dtos
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public List<string> RolesList { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Mail { get; set; }
        public string TelephoneNumber { get; set; }
        public BuildingInfoDto ResponsibleForBuilding { get; set; }
        public DateTime DOB { get; set; }
        public PositionDto LastSeenBuildingPosition { get; set; }
    }
}