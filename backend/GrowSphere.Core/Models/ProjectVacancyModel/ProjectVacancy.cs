using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.SkillModel;

namespace GrowSphere.Domain.Models.ProjectVacancyModel;

public class ProjectVacancy
{
    private readonly List<ProjectVacancySkill> _skills = new();

    private ProjectVacancy() { }
    
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public IReadOnlyCollection<ProjectVacancySkill> Skills => _skills;

    public DateTime CreatedAt { get; private set; }
    
    public static Result<ProjectVacancy, Error> Create(Guid projectId, Title title, Description description)
    {
        if (projectId == Guid.Empty)
            return Errors.General.ValueIsRequired("projectId");

        if (string.IsNullOrWhiteSpace(title.Value))
            return Errors.General.ValueIsRequired("title");

        var vacancy = new ProjectVacancy
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            Title = title,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };

        return vacancy;
    }
    
    public Result AddSkill(SkillId skillId)
    {
        if (_skills.Any(s => s.SkillId == skillId))
            return Result.Failure("Skill already added");

        _skills.Add(new ProjectVacancySkill(Id, skillId));
        return Result.Success();
    }
}