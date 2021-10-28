using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FloorController : ControllerBase
    {
        private readonly ICompartmentCRUDService<FloorDto, Floor> floorService;
        private readonly IMapper mapper;

        public FloorController(ICompartmentCRUDService<FloorDto, Floor> floorService, IMapper mapper)
        {
            this.floorService = floorService;
            this.mapper = mapper;
        }


        [HttpPost("addFloorToBuilding/{buildingId}")]
        public async Task<IActionResult> AddFloorToBuilding(int buildingId, [FromBody] FloorDto newFloorDto)
        {
            var result = await floorService.AddCompartment(buildingId, newFloorDto);
            var resultToDto = mapper.Map<FloorDto>(result);

            return Ok(result);
        }

        [HttpPut("changeFloorInfo/{floorId}")]
        public async Task<IActionResult> ChangeFloorInfo(int floorId, [FromBody] FloorDto newFloorDto)
        {
            var result = await floorService.ChangeCompartmentInfo(floorId, newFloorDto);
            var resultToDto = mapper.Map<FloorDto>(result);
            return Ok();
        }

        public async Task<IActionResult> RemoveFloorFromBuilding()
        {
            //TODO: implement removal in floor controller
            return Ok();
        }
    }
}