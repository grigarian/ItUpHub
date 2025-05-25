using CSharpFunctionalExtensions;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain.Models.NotificationModel;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Application.Users;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationHub _notificationHub;
    private readonly IProjectRepository _projectRepository;
    
    public NotificationService(
        INotificationRepository notificationRepository,
        INotificationHub notificationHub,
        IProjectRepository projectRepository)
    {
        _notificationRepository = notificationRepository;
        _notificationHub = notificationHub;
        _projectRepository = projectRepository;
    }
    
    public async Task<Result<NotificationId, Error>> SendNotificationAsync(UserId userId, string message, CancellationToken cancellationToken = default)
    {
        var notificationId = NotificationId.NewId();
        var notificationResult = Notification.Create(notificationId, userId, message);
        if (notificationResult.IsFailure)
            return notificationResult.Error;

        await _notificationRepository.AddAsync(notificationResult.Value, cancellationToken);
        await _notificationRepository.SaveChangesAsync(cancellationToken);

        // Заменяем прямой вызов на интерфейс
        await _notificationHub.SendNotificationAsync(userId.Value, message);

        return notificationId;
    }
    
    public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _notificationRepository.GetByUserIdAsync(userId, cancellationToken);
    }

    public async Task MarkNotificationAsReadAsync(NotificationId notificationId, CancellationToken cancellationToken = default)
    {
        await _notificationRepository.MarkAsReadAsync(notificationId, cancellationToken);
        await _notificationRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result<Guid, Error>> NotifyProjectManagersAsync(Guid projectId, string message, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetById(projectId,cancellationToken);
        if (project.IsFailure)
            return project.Error;

        var members = await _projectRepository.GetMembers(project.Value.Id, cancellationToken);
        
        var managerIds = members.Value
            .Where(m => m.Role == MemberRole.ProjectManager.ToString())
            .Select(m => m.UserId)
            .ToList();

        var notificationId = NotificationId.NewId();
        
        foreach (var userId in managerIds)
        {
            var notification = Notification.Create(
                notificationId,
                UserId.Create(userId), 
                message
            );

            await _notificationRepository.AddAsync(notification.Value, cancellationToken);

            await _notificationHub.SendNotificationAsync(userId, message);
        }

        return notificationId.Value;
    }
}