using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;
using FireSaverApi.Helpers;
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
        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        public async Task<IActionResult> AddRoomToFloor(int floorId, [FromBody] RoomDto newRoomDto)
        {
            var result = await roomService.AddCompartment(floorId, newRoomDto);
            var resultToDto = mapper.Map<RoomDto>(result);

            return Ok(resultToDto);
        }

        [HttpPut("changeRoomInfo/{roomId}")]
        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        public async Task<IActionResult> ChangeRoomInfo(int roomId, [FromBody] RoomDto newRoomDto)
        {
            var result = await roomService.ChangeCompartmentInfo(roomId, newRoomDto);
            var resultToDto = mapper.Map<RoomDto>(result);
            return Ok(resultToDto);
        }

        [HttpDelete("{roomId}")]
        [Authorize(Roles = new string[] { UserRoleName.ADMIN, UserRoleName.AUTHORIZED_USER })]
        public async Task<IActionResult> RemoveRoomFromBuilding(int roomId)
        {
            await roomService.DeleteCompartment(roomId);
            return Ok(new ServerResponse()
            {
                Message = "Room is deleted"
            });
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetRoomInfo(int roomId)
        {
            var result = await roomService.GetCompartmentInfo(roomId);
            var resultToDto = mapper.Map<RoomDto>(result);
            return Ok(resultToDto);
        }

    }
}