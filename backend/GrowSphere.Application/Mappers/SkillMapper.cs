using GrowSphere.Application.DTOs;
using GrowSphere.Domain.Models.SkillModel;

namespace GrowSphere.Application.Mappers;

public static class SkillMapper
{
    public static SkillDto ToSkillDto(Skill skill)
        => new(skill.Id.Value, skill.Title.Value);
} 