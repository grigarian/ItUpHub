using Microsoft.AspNetCore.SignalR;

namespace GrowSphere.Hubs;

public class NotificationHub : Hub
{
    
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier; // теперь не null
        Console.WriteLine($"Пользователь подключился: {userId}");

        await base.OnConnectedAsync();
    }

    // Метод для вызова с сервера клиенту
    public async Task SendNotification(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
}