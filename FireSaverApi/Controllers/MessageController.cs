using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.Dtos.MessageDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService messageService;
        private readonly IUserContextService userContext;

        public MessageController(IMessageService messageService,
            IUserContextService userContext)
        {
            this.messageService = messageService;
            this.userContext = userContext;
        }

        [Authorize]
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendNewMessage([FromBody] InputMessageDto messageDto)
        {
            await messageService.SendMessage(messageDto.UserId, messageDto.MessageType);
            return Ok(new ServerResponse() { Message = "Message is send" });
        }

        [Authorize]
        [HttpDelete("DeleteMessage/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId, [FromBody] InputMessageDto messageDto)
        {
            var userId = userContext.GetUserContext().Id;

            await messageService.DeleteMessage(messageId, userId);
            return Ok(new ServerResponse() { Message = "Message is deleted" });
        }

        [Authorize]
        [HttpGet("allMessages")]
        public async Task<IActionResult> GetAllMessages()
        {
            var userId = userContext.GetUserContext().Id;
            var messages = await messageService.GetAllMessagesForBuilding(userId);
            return Ok(messages);
        }
    }
}