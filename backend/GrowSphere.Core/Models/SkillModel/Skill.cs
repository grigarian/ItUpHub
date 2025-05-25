using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.Share;

namespace GrowSphere.Domain.Models.SkillModel
{
    public class Skill : Entity<SkillId>
    {
        private Skill(SkillId id) : base(id) { }

        public Title Title { get; private set; }

        private Skill(
            SkillId skillId,
            Title title)
            : base(skillId)
        {
            Title = title;
        }

        public static Result<Skill, Error> Create(SkillId id, Title title)
        {
            return new Skill(id, title);
        }
    }
}
