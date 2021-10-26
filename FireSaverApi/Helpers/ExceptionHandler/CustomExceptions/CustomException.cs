using System;

namespace FireSaverApi.Helpers.ExceptionHandler.CustomExceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException():base("Can't find the user! Try again later"){}
    }

    public class WrongPasswordException:Exception
    {
        public WrongPasswordException():base("Password or mail is incorrect"){}
    }
}