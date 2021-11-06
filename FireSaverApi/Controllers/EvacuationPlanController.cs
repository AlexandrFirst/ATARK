using System;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Helpers;
using FireSaverApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EvacuationPlanController : ControllerBase
    {
        private readonly IEvacuationService evacuationService;
        private readonly IUserContextService userContextService;
        private readonly IUserHelper userHelper;

        public EvacuationPlanController(IEvacuationService evacuationService,
                                        IUserContextService userContextService,
                                        IUserHelper userHelper)
        {
            this.evacuationService = evacuationService;
            this.userContextService = userContextService;
            this.userHelper = userHelper;
        }

        [HttpPost("{compartmentId}/newEvacPlan")]
        [Authorize(Roles = new string[] { UserRole.ADMIN, UserRole.AUTHORIZED_USER })]
        public async Task<IActionResult> AddEvacuationPlanToCompartment(int compartmentId, [FromForm] IFormFile evacPlanImgae)
        {
            await CheckIsResponsible();

            var response = await evacuationService.addEvacuationPlanToCompartment(compartmentId, evacPlanImgae);
            return Ok(response);
        }

        [HttpPut("{compartmentId}/updateEvacPlan")]
        [Authorize(Roles = new string[] { UserRole.ADMIN, UserRole.AUTHORIZED_USER })]
        public async Task<IActionResult> ChangeEvacuationPlanOfCompartment(int compartmentId, [FromForm] IFormFile newEvacPlanImgae)
        {
            await CheckIsResponsible();

            var response = await evacuationService.changeEvacuationPlanOfCompartment(compartmentId, newEvacPlanImgae);
            return Ok(response);
        }

        [HttpGet("{compartmentId}")]
        public async Task<IActionResult> GetEvacuationPlanOfCompartment(int compartmentId)
        {
            var response = await evacuationService.GetEvacuationPlanOfCompartment(compartmentId);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("evacuationplans")]
        public async Task<IActionResult> GetEvacuationPlan()
        {
            var response = await evacuationService.GetEvacuationPlansFromCompartmentByUserId(userContextService.GetUserContext().Id);
            return Ok(response);
        }

        [HttpDelete("{compartmentId}")]
        [Authorize(Roles = new string[] { UserRole.ADMIN, UserRole.AUTHORIZED_USER })]
        public async Task<IActionResult> DeleteEvacuationPlanOfCompartment(int compartmentId)
        {
            await CheckIsResponsible();

            await evacuationService.RemoveEvacuationPlanOfCompartment(compartmentId);
            return Ok(new ServerResponse()
            {
                Message = "Evacuation plan is deleted successfully"
            });
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
            if (user.ResponsibleForBuilding != null || userContext.RolesList.Contains(UserRole.ADMIN))
            {
                return true;
            }

            return false;
        }
    }
}