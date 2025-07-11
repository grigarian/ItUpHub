using GrowSphere.Application.DTOs;
using GrowSphere.Domain.Models.ProjectModel;

namespace GrowSphere.Application.Mappers;

public static class ProjectMemberMapper
{
    public static ProjectMemberDto ToProjectMemberDto(ProjectMember member)
        => new ProjectMemberDto
        {
            UserId = member.UserId.Value,
            UserName = member.User?.Name.Value ?? string.Empty,
            Role = member.Role.ToString(),
            PictureUrl = member.User?.ProfilePicture?.Path
        };
} 