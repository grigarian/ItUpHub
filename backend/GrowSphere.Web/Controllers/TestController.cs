using GrowSphere.Application.Interfaces;
using GrowSphere.Web.RealTime;
using Microsoft.AspNetCore.Mvc;

namespace GrowSphere.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly INotificationHub _notificationHubSender;

    public TestController(INotificationHub notificationHubSender)
    {
        _notificationHubSender = notificationHubSender;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromQuery] Guid userId, [FromBody] string message)
    {
        if (string.IsNullOrEmpty(message))
            return BadRequest("Message cannot be empty.");

        await _notificationHubSender.SendNotificationAsync(userId, message);
        return Ok("Notification sent.");
    }
}