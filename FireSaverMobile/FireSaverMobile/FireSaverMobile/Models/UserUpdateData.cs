using FireSaverMobile.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models
{
    public class UserUpdateData: BaseViewModel
    {
        public UserUpdateData(string name,
            string surname, string patronymic, string telNumber,
            DateTime dob)
        {
            this.name = name;
            this.surname = surname;
            this.patronymic = patronymic;
            this.telNumber = telNumber;
        }

        private string name = "";

        public string Name { get { return name; } set { SetValue(ref name, value); } }

        private string surname = "";

        public string Surname { get { return surname; } set { SetValue(ref surname, value); } }

        private string patronymic = "";

        public string Patronymic { get { return patronymic; } set { SetValue(ref patronymic, value); } }

        private string telNumber = "";

        public string TelephoneNumber { get { return telNumber; } set { SetValue(ref telNumber, value); } }

        private DateTime dob = DateTime.Now;

        public DateTime Dob { get { return dob; } set { SetValue(ref dob, value); } }
    }
}
