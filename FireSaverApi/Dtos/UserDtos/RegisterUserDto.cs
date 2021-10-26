using System;

namespace FireSaverApi.Dtos
{
    public class RegisterUserDto: UserInfoDto
    {
        public string Password { get; set; } 
    }
}