using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.SkillModel;

namespace GrowSphere.Domain.Models.UserModel;

public class UserSkill
{
    public UserId UserId { get; private set; }
    public User User { get; private set; }

    public SkillId SkillId { get; private set; }
    public Skill Skill { get; private set; }

    private UserSkill() { } // EF

    private UserSkill(UserId userId, SkillId skillId)
    {
        UserId = userId;
        SkillId = skillId;
    }

    public static Result<UserSkill, Error> Create(UserId userId, SkillId skillId)
    {
        if(userId.Value == Guid.Empty)
            return Errors.General.ValueIsRequired("UserId");
        
        if(skillId.Value == Guid.Empty)
            return Errors.General.ValueIsRequired("SkillId");
        
        return new UserSkill(userId, skillId);
    }
}