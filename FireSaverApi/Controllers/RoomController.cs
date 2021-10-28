using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;
using FireSaverApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ICompartmentService<RoomDto, Room> roomService;
        private readonly IMapper mapper;

        public RoomController(ICompartmentService<RoomDto, Room> roomService, IMapper mapper)
        {
            this.roomService = roomService;
            this.mapper = mapper;
        }

        [HttpPost("addRoomToFloor/{floorId}")]
        public async Task<IActionResult> AddRoomToFloor(int floorId, [FromBody] RoomDto newRoomDto)
        {
            var result = await roomService.AddCompartment(floorId, newRoomDto);
            var resultToDto = mapper.Map<RoomDto>(result);

            return Ok(resultToDto);
        }

        [HttpPut("changeRoomInfo/{roomId}")]
        public async Task<IActionResult> ChangeFloorInfo(int floorId, [FromBody] RoomDto newRoomDto)
        {
            var result = await roomService.ChangeCompartmentInfo(floorId, newRoomDto);
            var resultToDto = mapper.Map<RoomDto>(result);
            return Ok(resultToDto);
        }

        [HttpDelete("{roomId}")]
        public async Task<IActionResult> RemoveFloorFromBuilding(int roomId)
        {
            await roomService.DeleteCompartment(roomId);
            return Ok(new ServerResponse()
            {
                Message = "Floor is deleted"
            });
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetFloorInfo(int roomId)
        {
            var result = await roomService.GetCompartmentInfo(roomId);
            var resultToDto = mapper.Map<RoomDto>(result);
            return Ok(resultToDto);
        }

    }
}