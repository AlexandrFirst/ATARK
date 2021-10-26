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

    public class UserContextNotFoundException:Exception
    {
        public UserContextNotFoundException():base("Can't find user context. Request should be authorized"){}
    }

    public class InorrectOldPasswordException:Exception
    {
        public InorrectOldPasswordException():base("You have entered wrong old password"){}
    }
}