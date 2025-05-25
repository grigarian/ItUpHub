using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Domain.Models.NotificationModel;

public class Notification: Entity<NotificationId>
{
    private Notification(NotificationId id) : base(id) { }
    public UserId UserId { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public bool IsRead { get; private set; } = false;
    
    private Notification(NotificationId id,
        UserId userId,
        string message,
        DateTime createdAt) : base(id)
    {
        UserId = userId;
        Message = message;
        CreatedAt = createdAt;
        IsRead = false;
    }

    public static Result<Notification, Error> Create(NotificationId id,
        UserId userId,
        string message)
    {
        if (userId.Value == Guid.Empty)
            return Errors.General.NotFound(userId.Value);
        
        if (id.Value == Guid.Empty)
            return Errors.General.NotFound(id.Value);
        
        if(string.IsNullOrWhiteSpace(message))
            return Errors.General.ValueIsRequired("message");
        
        return new Notification(id, userId, message, DateTime.UtcNow);
    }
    
    public void MarkAsRead() => IsRead = true;
}