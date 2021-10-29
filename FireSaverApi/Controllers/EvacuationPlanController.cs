using System.Threading.Tasks;
using FireSaverApi.Contracts;
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
        private readonly IUserContextService userContext;

        public EvacuationPlanController(IEvacuationService evacuationService,
                                        IUserContextService userContext)
        {
            this.evacuationService = evacuationService;
            this.userContext = userContext;
        }

        [HttpPost("{compartmentId}/newEvacPlan")]
        public async Task<IActionResult> AddEvacuationPlanToCompartment(int compartmentId, [FromForm] IFormFile evacPlanImgae)
        {
            var response = await evacuationService.addEvacuationPlanToCompartment(compartmentId, evacPlanImgae);
            return Ok(response);
        }

        [HttpPut("{compartmentId}/updateEvacPlan")]
        public async Task<IActionResult> ChangeEvacuationPlanOfCompartment(int compartmentId, [FromForm] IFormFile newEvacPlanImgae)
        {
            var response = await evacuationService.changeEvacuationPlanOfCompartment(compartmentId, newEvacPlanImgae);
            return Ok(response);
        }

        [HttpGet("{compartmentId}")]
        public async Task<IActionResult> GetEvacuationPlanOfCompartment(int compartmentId)
        {
            var response = await evacuationService.getEvacuationPlanofCompartment(compartmentId);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("evacuationplans")]
        public async Task<IActionResult> GetEvacuationPlan()
        {
            var response = await evacuationService.GetEvacuationPlansFromCompartmentByUserId(userContext.GetUserContext().Id);
            return Ok(response);
        }

        [HttpGet("evacuationPlansUnothorized/{compartmentId}")]
        public async Task<IActionResult> GetEvacuationPlanUnothorized(int compartmentId)
        {
            var response = await evacuationService.GetEvacuationPlansFromCompartmentByCompartmentId(compartmentId);
            return Ok(response);
        }

        [HttpDelete("{compartmentId}")]
        public async Task<IActionResult> DeleteEvacuationPlanOfCompartment(int compartmentId)
        {
            await evacuationService.removeEvacuationPlanOfCompartment(compartmentId);
            return Ok(new ServerResponse()
            {
                Message = "Evacuation plan is deleted successfully"
            });
        }


    }
}