using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.SkillModel;

namespace GrowSphere.Domain.Models.ProjectVacancyModel;

public class ProjectVacancySkill
{
    public Guid ProjectVacancyId { get; private set; }
    public ProjectVacancy ProjectVacancy { get; private set; }

    public SkillId SkillId { get; private set; }
    public Skill Skill { get; private set; } 

    public ProjectVacancySkill() { } // EF

    public ProjectVacancySkill(Guid vacancyId, SkillId skillId)
    {
        ProjectVacancyId = vacancyId;
        SkillId = skillId;
    }

    public static Result<ProjectVacancySkill, Error> Create(Guid vacancyId, SkillId skillId)
    {
        if (vacancyId == Guid.Empty)
            return Errors.General.ValueIsRequired("vacancyId");

        if (skillId.Value == Guid.Empty)
            return Errors.General.ValueIsRequired("skillId");
        
        return new ProjectVacancySkill(vacancyId, skillId);
    }
}