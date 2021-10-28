using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        public RoomController()
        {

        }

        public async Task<IActionResult> AddRoomToFloor()
        {
            return Ok();
        }

        public async Task<IActionResult> ChangeRoomInfo()
        {
            return Ok();
        }

        public async Task<IActionResult> RemoveRoomFromFloor()
        {
            return Ok();
        }
        

    }
}