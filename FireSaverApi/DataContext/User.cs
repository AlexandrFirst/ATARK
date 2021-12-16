using System;
using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class UserRoleName
    {
        public const string ADMIN = "Admin";
        public const string AUTHORIZED_USER = "AuthorizedUser";
        public const string GUEST = "Guest";
    }

    public class User
    {
        public User()
        {
            RolesList = new List<UserRole>();
            Messages = new List<Message>();
        }
        public int Id { get; set; }
        public IList<UserRole> RolesList { get; set; }
        public IList<Message> Messages { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string TelephoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public string LastSeenBuildingPosition { get; set; }
        public Building ResponsibleForBuilding { get; set; }
        public Compartment CurrentCompartment { get; set; }

    }
}