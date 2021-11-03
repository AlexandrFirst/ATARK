using FireSaverApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IoTController:ControllerBase
    {
        private readonly IoTService iotService;

        public IoTController(IoTService iotService)
        {
            this.iotService = iotService;
        }
    }
}