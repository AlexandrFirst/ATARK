using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScalePointsController : ControllerBase
    {

        private readonly IScalePointService scalePointService;

        public ScalePointsController(IScalePointService scalePointService)
        {
            this.scalePointService = scalePointService;

        }

        [HttpPost("newpos/{evacPlanId}")]
        public async Task<IActionResult> WriteNewPosition(int evacPlanId, [FromBody] ScalePointDto inputPoint)
        {
            var response = await scalePointService.AddNewScalePoint(evacPlanId, inputPoint);
            return Ok(response);
        }


        [HttpDelete("points/{evacPlanId}")]
        public async Task<IActionResult> DeleteAllPoints(int evacPlanId)
        {
            await scalePointService.DeleteAllPoints(evacPlanId);

            return Ok(new ServerResponse { Message = "All points are deleted" });
        }

        [HttpDelete("points/singlePoint/{scalePointId}")]
        public async Task<IActionResult> DeleteSingleScalePoints(int scalePointId)
        {
            await scalePointService.DeleteSinglePoint(scalePointId);

            return Ok(new ServerResponse { Message = "Point is deleted" });
        }
    }
}