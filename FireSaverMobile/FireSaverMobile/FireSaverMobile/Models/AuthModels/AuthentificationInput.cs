using FireSaverMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models
{
    public class AuthentificationInput:BaseViewModel
    {
        private string mail = "";
        public string Mail { get { return mail; } set { SetValue(ref mail, value); } }

        private string pass = "";
        public string Password { get { return pass; } set { SetValue(ref pass, value); } }
    }
}
