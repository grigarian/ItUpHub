using CSharpFunctionalExtensions;
using GrowSphere.Core;

namespace GrowSphere.Domain.Models;

public class JoinRequest
{
    private JoinRequest() { }

    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid UserId { get; private set; }
    public string Message { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public JoinRequestStatus Status { get; private set; }
    public string? ManagerComment { get; private set; }

    public static Result<JoinRequest, Error> Create(Guid projectId, Guid userId, string message)
    {
        if (projectId == Guid.Empty)
            return Errors.General.ValueIsRequired("projectId");

        if (userId == Guid.Empty)
            return Errors.General.ValueIsRequired("userId");

        if (string.IsNullOrWhiteSpace(message))
            return Errors.General.ValueIsRequired("message");

        var joinRequest = new JoinRequest
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            UserId = userId,
            Message = message.Trim(),
            CreatedAt = DateTime.UtcNow,
            Status = JoinRequestStatus.Pending
        };

        return joinRequest;
    }

    public Result Approve(string? comment = null)
    {
        if (Status != JoinRequestStatus.Pending)
            return Result.Failure("Request is not pending");

        Status = JoinRequestStatus.Approved;
        ManagerComment = comment;
        return Result.Success();
    }

    public Result Reject(string? comment = null)
    {
        if (Status != JoinRequestStatus.Pending)
            return Result.Failure("Request is not pending");

        Status = JoinRequestStatus.Rejected;
        ManagerComment = comment;
        return Result.Success();
    }
}

public enum JoinRequestStatus
{
    Pending,
    Approved,
    Rejected
}