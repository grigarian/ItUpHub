using GrowSphere.Application.Users;
using GrowSphere.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Web.Controllers;

[ApiController]
[Route("notifications")]
public class NotificationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NotificationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET /notifications
    [HttpGet]
    public async Task<IActionResult> GetUserNotifications(
        [FromServices] NotificationService notificationService,
        [FromServices] CurrentUserService service)
    {
        var userId = service.UserId;
        
        var notifications = await notificationService.GetUserNotificationsAsync(userId);

        return Ok(notifications);
    }

    // POST /notifications/mark-all-read
    [HttpPost("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead([FromServices] CurrentUserService service)
    {
        var userId = service.UserId;

        var unreadNotifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        if (!unreadNotifications.Any())
            return Ok();

        foreach (var notification in unreadNotifications)
        {
            notification.MarkAsRead();
        }

        await _context.SaveChangesAsync();

        return Ok();
    }
}