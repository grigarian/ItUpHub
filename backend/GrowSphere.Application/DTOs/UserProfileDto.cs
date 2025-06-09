using GrowSphere.Domain.Models.SkillModel;

namespace GrowSphere.Application.DTOs;

public record UserProfileDto(Guid id,
    string UserName,
    string Email,
    string Bio,
    string Avatar,
    bool IsAdmin,
    IEnumerable<Skill> Skills);