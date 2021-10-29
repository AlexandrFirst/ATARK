using System.Threading.Tasks;
using FireSaverApi.Contracts;
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

        public EvacuationPlanController(IEvacuationService evacuationService)
        {
            this.evacuationService = evacuationService;
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
            var response = await evacuationService.changeEvacuationPlanToCompartment(compartmentId, newEvacPlanImgae);
            return Ok(response);
        }

        [HttpGet("{compartmentId}")]
        public async Task<IActionResult> GetEvacuationPlanOfCompartment(int compartmentId)
        {
            var response = await evacuationService.getEvacuationPlanToCompartment(compartmentId);
            return Ok(response);
        }

        [HttpDelete("{compartmentId}")]
        public async Task<IActionResult> DeleteEvacuationPlanOfCompartment(int compartmentId)
        {
            await evacuationService.removeEvacuationPlanToCompartment(compartmentId);
            return Ok(new ServerResponse()
            {
                Message = "Evacuation plan is deleted successfully"
            });
        }
    }
}