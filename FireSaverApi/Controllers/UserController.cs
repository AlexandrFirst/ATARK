using System.Collections.Generic;
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
            var registeredUser = await userService.CreateNewUser(newUserInfo, UserRole.AUTHORIZED_USER);

            return Ok(registeredUser);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> AuthUser([FromBody] AuthUserDto authUserInfo)
        {
            var authResponse = await authService.AuthUser(authUserInfo);

            return Ok(authResponse);

        }

        [HttpGet("guestAuth")]
        public async Task<IActionResult> AuthGuestUser()
        {
            var authResponse = await authService.AuthGuest();

            return Ok(authResponse);

        }

        [Authorize(Roles = new string[] { UserRole.GUEST })]
        [HttpGet("guestAuth/{userId}")]
        public async Task<IActionResult> LogoutGuestUser(int userId)
        {
            await authService.LogoutGuest(userId);

            return Ok(new ServerResponse()
            {
                Message = "Guest is logged out"
            });

        }


        [Authorize(Roles = new string[] { UserRole.ADMIN })]
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

            return Ok(new ServerResponse() { Message = "Password is updated" });
        }

        [Authorize]
        [HttpGet("evacuate/{fromCompartmentId}")]
        public async Task<IActionResult> GetEvacuationRouteForCompartment(int fromCompartmentId)
        {
            var currentUserId = userContextService.GetUserContext().Id;
            var response = await userService.BuildEvacuationRootForCompartment(fromCompartmentId);

            return Ok(new ServerResponse() { Message = "Password is updated" });
        }

        [Authorize]
        [HttpPost("enter")]
        public async Task<IActionResult> EnterCompartment([FromBody] UserEnterCompartmentDto enterCompartmentDto)
        {
            int userId = userContextService.GetUserContext().Id;
            var testOutput = await userService.EnterCompartmentById(userId,
                                                                    enterCompartmentDto.compartmentId,
                                                                    enterCompartmentDto.iotId);
            if (testOutput == null)
            {
                return Ok();
            }
            else
            {
                return Ok(testOutput);
            }
        }

        [Authorize]
        [HttpGet("alarm")]
        public async Task<IActionResult> SetAlarm()
        {
            var userId = userContextService.GetUserContext().Id;
            await userService.SetAlaramForBuilding(userId);
            return Ok(new ServerResponse() { Message = "Alarm is set" });
        }
    }
}