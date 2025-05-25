using GrowSphere.Domain.Models.NotificationModel;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Application.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<List<Notification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task MarkAsReadAsync(NotificationId notificationId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}