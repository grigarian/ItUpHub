namespace GrowSphere.Application.DTOs;

public record ProjectMemberDto
{
    public Guid UserId { get; init; }
    public string UserName { get; init; }
    public string Role { get; init; }
    
    public string? PictureUrl { get; init; }
}