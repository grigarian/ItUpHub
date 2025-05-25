using GrowSphere.Application.Interfaces;
using GrowSphere.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GrowSphere.Web.RealTime;

public class NotificationHubSender: INotificationHub
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationHubSender(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task SendNotificationAsync(Guid userId, string message)
    {
        Console.WriteLine($"Отправка уведомления пользователю {userId}, сообщение: '{message}'");
        return _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);
    }
}