using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Domain.Models.ProjectMessage;

public class ProjectMessage: Entity<ProjectMessageId>
{
    private ProjectMessage(ProjectMessageId id): base(id) { }
    public const int CONTENT_MAX_LENGTH = 2000;
    
    public ProjectId ProjectId { get; private set; }
    public UserId SenderId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime SentAt { get; private set; } = DateTime.UtcNow;

    public User Sender { get; private set; } = null!;
    public Project Project { get; private set; } = null!;

    private ProjectMessage(
        ProjectMessageId id,
        ProjectId projectId,
        UserId senderId,
        string content,
        DateTime sentAt) : base(id)
    {
        Id = id;
        ProjectId = projectId;
        SenderId = senderId;
        Content = content;
        SentAt = sentAt;
    }

    public static Result<ProjectMessage, Error> Create(
        ProjectMessageId id,
        ProjectId projectId,
        UserId senderId,
        string content)
    {
        if(projectId == Guid.Empty)
            return Errors.General.ValueIsRequired("projectId");
        if(senderId == Guid.Empty)
            return Errors.General.ValueIsRequired("senderId");
        if(content.Length > CONTENT_MAX_LENGTH)
            return Errors.General.ValueIsInvalid("content");
        
        return new ProjectMessage(id,projectId, senderId, content, DateTime.UtcNow);
    }
}