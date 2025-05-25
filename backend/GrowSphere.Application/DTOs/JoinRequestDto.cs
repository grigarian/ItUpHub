using GrowSphere.Domain.Models.ProjectModel;

namespace GrowSphere.Application.DTOs;

public class JoinRequestDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string Message { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = default!;
    public string? ManagerComment { get; set; }
}

public record ApproveJoinRequestDto(string Role);
public record RejectJoinRequestDto(string Reason);