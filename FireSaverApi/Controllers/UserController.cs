using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.Dtos;
using FireSaverApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthUserService authService;

        public UserController(IUserService userService, IAuthUserService authService)
        {
            this.authService = authService;
            this.userService = userService;
        }

        [HttpPost("newuser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto newUserInfo)
        {
            var registeredUser = await userService.CreateNewUser(newUserInfo);

            return Ok(registeredUser);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> AuthUser([FromBody] AuthUserDto authUserInfo)
        {
            var authResponse = await authService.AuthUser(authUserInfo);

            return Ok(authResponse);

        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            var userInfo = await userService.GetUserInfoById(userId);
            return Ok(userInfo);
        }

    }
}