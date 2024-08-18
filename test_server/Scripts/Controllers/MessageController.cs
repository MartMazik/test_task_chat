using Microsoft.AspNetCore.Mvc;
using TestServer.Scripts.Common.Logs;
using TestServer.Scripts.Models;
using TestServer.Scripts.Service;

namespace TestServer.Scripts.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController(IMessageService messageService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendMessage(MessageModel messageModel)
    {
        var result = await messageService.SendMessage(messageModel);
        if (result.IsError)
        {
            Logger.Error(result.Message, "SendMessage");
            return BadRequest(result.Message);
        }
        
        Logger.Info("Successfully", "SendMessage");
        return Ok(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetMessages(DateTime startDate, DateTime endDate)
    {
        var result = await messageService.GetMessages(startDate, endDate);

        if (result.IsError)
        {
            Logger.Error($"{result.Message}", "GetMessages");
            return BadRequest(result.Message);
        }

        if (result.Data is { Count: 0 })
        {
            Logger.Info($"Executed, no messages found between {startDate} - {endDate}", "GetMessages");
            return NoContent();
        }
        
        Logger.Info($"Successfully, found {result.Data!.Count} messages", "GetMessages");
        return Ok(result.Data);
    }
}