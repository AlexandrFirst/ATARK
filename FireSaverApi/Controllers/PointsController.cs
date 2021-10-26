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
    public class PointsController : ControllerBase
    {
        private readonly DatabaseContext context;
        private readonly IMapper mapper;
        private readonly ILocationService locationService;

        public PointsController(DatabaseContext context,
                                IMapper mapper,
                                ILocationService locationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.locationService = locationService;
        }

        // [HttpPost("newpos")]
        // public async Task<IActionResult> WriteNewPosition([FromBody] PointDto inputPoint)
        // {
        //     var pointToInsert = mapper.Map<ScalePoint>(inputPoint);
        //     await context.ScalePoints.AddAsync(pointToInsert);
        //     await context.SaveChangesAsync();
        //     return Ok(pointToInsert);
        // }

        // [HttpGet("calculatePositionModel")]
        // public async Task<IActionResult> WriteNewPosition()
        // {
        //     LocationPointModel locationModel = await locationService.CalculateLocationModel();
        //     return Ok(locationModel);
        // }

        // [HttpPost("mapPos")]
        // public async Task<IActionResult> ConvertWorldToImgPos([FromBody] PositionDto inputPosition)
        // {
        //     PositionDto imgPos = await locationService.WorldToImgPostion(inputPosition);
        //     return Ok(imgPos);
        // }

        // [HttpPost("imgPos")]
        // public async Task<IActionResult> ConvertImgToWorldPos([FromBody] PositionDto inputPosition)
        // {
        //     PositionDto imgPos = await locationService.ImgToWorldPostion(inputPosition);
        //     return Ok(imgPos);
        // }

        // [HttpDelete("points")]
        // public async Task<IActionResult> DeleteAllPoints()
        // {
        //     context.ScalePoints.RemoveRange(context.ScalePoints);
        //     await context.SaveChangesAsync();

        //     return Ok(new { Message = "All points are deleted" });
        // }


    }
}