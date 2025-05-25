namespace GrowSphere.Application.Interfaces;

public interface INotificationHub
{
    Task SendNotificationAsync(Guid userId, string message);
}