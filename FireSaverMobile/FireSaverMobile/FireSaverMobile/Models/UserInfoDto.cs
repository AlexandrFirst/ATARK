using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models
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
        public DateTime DOB { get; set; }
        public Position LastSeenBuildingPosition { get; set; }
    }
}
