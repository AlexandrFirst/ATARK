using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Helpers;
using FireSaverApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthUserService authService;
        private readonly IUserContextService userContextService;

        public UserController(IUserService userService,
                            IAuthUserService authService,
                            IUserContextService userContextService)
        {
            this.userContextService = userContextService;
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

        [Authorize(Role = UserRole.ADMIN)]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            var userInfo = await userService.GetUserInfoById(userId);
            return Ok(userInfo);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfoDto currentUserInfo)
        {
            var currentUserId = userContextService.GetUserContext().Id;
            currentUserInfo.Id = currentUserId;

            var updatedUserInfo = await userService.UpdateUserInfo(currentUserInfo);
            return Ok(updatedUserInfo);
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] NewUserPasswordDto newUserPassword)
        {
            var currentUserId = userContextService.GetUserContext().Id;
            await userService.ChangeOldPassword(currentUserId, newUserPassword);

            return Ok(new ServerResponse(){ Message = "Password is updated" });
        }

        [Authorize]
        [HttpGet("evacuate/{fromCompartmentId}")]
        public async Task<IActionResult> GetEvacuationRouteForCompartment(int fromCompartmentId)
        {
            var currentUserId = userContextService.GetUserContext().Id;
            var response = await userService.BuildEvacuationRootForCompartment(fromCompartmentId);

            return Ok(new ServerResponse(){ Message = "Password is updated" });
        }



    }
}