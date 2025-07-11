using GrowSphere.Application.DTOs;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Application.Mappers;

public static class UserMapper
{
    public static UserProfileDto ToUserProfileDto(User user)
    {
        return new UserProfileDto(
            user.Id.Value,
            user.Name.Value,
            user.Email.Value,
            user.Bio.Value,
            user.ProfilePicture?.Path ?? string.Empty,
            user.IsAdmin,
            user.Skills?.Select(s => new SkillDto(s.Skill.Id.Value, s.Skill.Title.Value)).ToList() ?? new List<SkillDto>()
        );
    }
} 