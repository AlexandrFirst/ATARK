using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.Dtos.TestDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService testService;

        public TestController(ITestService testService)
        {
            this.testService = testService;
        }

        [HttpPost("addCompartmentTest/{compartmentId}")]
        public async Task<IActionResult> AddTestToCompartment(int compartmentId, [FromBody] TestInputDto newTest)
        {
            var response = await testService.AddTestToCompartment(compartmentId, newTest);
            return Ok(response);
        }

        [HttpDelete("removeCompartmentTest/{compartmentId}")]
        public async Task<IActionResult> RemoveTestFromCompartment(int compartmentId)
        {
            await testService.RemoveTestFromCompartment(compartmentId);
            return Ok(new ServerResponse() { Message = "Test is removed" });
        }

        [HttpPut("updateCompartmentTest/{testId}")]
        public async Task<IActionResult> UpdateCompartmentTest(int testId, [FromBody] TestInputDto newTest)
        {
            var response = await testService.UpdateTestToCompartment(testId, newTest);
            return Ok(response);
        }

        [HttpPost("answerCompartmentTest")]
        public async Task<IActionResult> AnswerCompartmentTest([FromBody] AnswerListDto testAnswers)
        {
            var success = await testService.CheckTestAnwears(testAnswers);
            if (success)
                return Ok(new ServerResponse() { Message = "Test is passed" });
            else
                return Ok(new ServerResponse() { Message = "Test is not passed" });
        }
    }
}