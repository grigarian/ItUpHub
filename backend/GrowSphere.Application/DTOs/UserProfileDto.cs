using GrowSphere.Application.DTOs;

namespace GrowSphere.Application.DTOs;

public record UserProfileDto(Guid id,
    string UserName,
    string Email,
    string Bio,
    string Avatar,
    bool IsAdmin,
    IEnumerable<SkillDto> Skills);