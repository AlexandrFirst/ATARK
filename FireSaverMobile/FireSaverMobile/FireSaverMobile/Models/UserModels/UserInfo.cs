using FireSaverMobile.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models
{
    public class UserInfo : UserUpdateData
    {
        public UserInfo(int id, string mail, Position pos, string name, 
            string surname, string patronymic, string telNumber,
            DateTime dob): base(name, surname, patronymic, telNumber, dob)
        {
            this.id = id;
            this.mail = mail;
            this.lastSeenBuildingPosition = pos;
           
        }


        private int id = -1;
        public int Id { get { return id; } set { SetValue(ref id, value); } }

        private string mail = "";
        public string Mail { get { return mail; } set { SetValue(ref mail, value); } }


        private Position lastSeenBuildingPosition = new Position();
        public Position LastSeenBuildingPosition
        {
            get { return lastSeenBuildingPosition; }
            set { SetValue(ref lastSeenBuildingPosition, value); }
        }
    }
}
