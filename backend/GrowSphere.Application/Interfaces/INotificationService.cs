using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.NotificationModel;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Application.Interfaces;

public interface INotificationService
{
    Task<Result<NotificationId, Error>> SendNotificationAsync(UserId userId, string message, CancellationToken cancellationToken = default);

    Task<List<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task MarkNotificationAsReadAsync(NotificationId notificationId, CancellationToken cancellationToken = default);
    
    Task<Result<Guid, Error>> NotifyProjectManagersAsync(
        Guid projectId,
        string message,
        CancellationToken cancellationToken = default);
}