using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.Dtos;
using FireSaverApi.Dtos.IoTDtos;
using FireSaverApi.Models;
using FireSaverApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IoTController : ControllerBase
    {
        private readonly IIoTService iotService;

        public IoTController(IIoTService iotService)
        {
            this.iotService = iotService;
        }

        [HttpPost("newIot")]
        public async Task<IActionResult> AddNewIoT([FromBody] NewIoTDto newIoT)
        {
            await iotService.AddIoTToDB(newIoT);
            return Ok(new ServerResponse() { Message = "Iot with id: " + newIoT.IotIdentifier + " added" });
        }


        [HttpPost("newIotToCompartment")]
        public async Task<IActionResult> AddNewIoTToCompartment([FromBody] NewIoTCompartmentDto newIoT)
        {
            await iotService.AddIoTToCompartment(newIoT.CompartmentId, newIoT.IoTIdentifier);
            return Ok(new ServerResponse() { Message = "Iot with id: " + newIoT.IoTIdentifier + " added to comartment with id: " + newIoT.CompartmentId });
        }

        [HttpPost("loginIot")]
        public async Task<IActionResult> IoTLogin([FromBody] LoginIoTDto loginIoTDto)
        {
            var response = await iotService.LoginIot(loginIoTDto);
            return Ok(response);
        }

        [HttpDelete("removeIoTFromCompartment/{iotId}/{compartmentId}")]
        public async Task<IActionResult> RemoveIoTFromCompartment(string iotId, int compartmentId)
        {
            await iotService.RemoveIoTFromCompartment(compartmentId, iotId);
            return Ok(new ServerResponse() { Message = "Iot with id: " + iotId + " removed comartment with id: " + compartmentId });
        }

        [HttpPut("updateIotPos/{iotId}")]
        public async Task<IActionResult> UpdateIoTPostion(string iotId, [FromBody] PositionDto newPos)
        {
            var response = await iotService.UpdateIoTPostion(iotId, newPos);
            return Ok(response);

        }
    }
}