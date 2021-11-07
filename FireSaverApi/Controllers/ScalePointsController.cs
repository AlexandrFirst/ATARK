using System;
using System.Threading.Tasks;
using AutoMapper;
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
    [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
    public class ScalePointsController : ControllerBase
    {

        private readonly IScalePointService scalePointService;
        private readonly IUserHelper userHelper;
        private readonly IUserContextService userContextService;

        public ScalePointsController(IScalePointService scalePointService,
                                    IUserContextService userContextService,
                                    IUserHelper userHelper)
        {
            this.userContextService = userContextService;
            this.userHelper = userHelper;
            this.scalePointService = scalePointService;



        }

        [HttpPost("newpos/{evacPlanId}")]
        public async Task<IActionResult> WriteNewPosition(int evacPlanId, [FromBody] ScalePointDto inputPoint)
        {
            await CheckIsResponsible();
            var response = await scalePointService.AddNewScalePoint(evacPlanId, inputPoint);
            return Ok(response);
        }


        [HttpDelete("points/{evacPlanId}")]
        public async Task<IActionResult> DeleteAllPoints(int evacPlanId)
        {
            await CheckIsResponsible();
            await scalePointService.DeleteAllPoints(evacPlanId);

            return Ok(new ServerResponse { Message = "All points are deleted" });
        }

        [HttpDelete("points/singlePoint/{scalePointId}")]
        public async Task<IActionResult> DeleteSingleScalePoints(int scalePointId)
        {
            await CheckIsResponsible();
            await scalePointService.DeleteSinglePoint(scalePointId);

            return Ok(new ServerResponse { Message = "Point is deleted" });
        }

        async Task CheckIsResponsible()
        {
            var userContext = userContextService.GetUserContext();
            if (!await IsUserHaveRightsToChangeEvacPlan(userContext))
            {
                throw new Exception("You are not responsible user");
            }
        }

        async Task<bool> IsUserHaveRightsToChangeEvacPlan(MyHttpContext userContext)
        {
            var user = await userHelper.GetUserById(userContext.Id);
            if (user.ResponsibleForBuilding != null || userContext.RolesList.Contains(UserRoleName.ADMIN))
            {
                return true;
            }

            return false;
        }
    }
}