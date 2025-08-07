using CSharpFunctionalExtensions;
using GrowSphere.Core;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.UserModel;
using GrowSphere.Domain.Models.ProjectModel;

namespace GrowSphere.Domain.Models.ProjectVacancyModel;

public enum ProjectVacancyStatus
{
    Open = 0,
    Closed = 1,
    Archived = 2
}

public class ProjectVacancy
{
    private readonly List<Skill> _skills = new();
    private readonly List<VacancyApplication> _vacancyApplications = new();

    private ProjectVacancy() { }
    
    public Guid Id { get; private set; }
    public ProjectId ProjectId { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public IReadOnlyCollection<Skill> Skills => _skills.AsReadOnly();
    public IReadOnlyCollection<VacancyApplication> VacancyApplications => _vacancyApplications.AsReadOnly();
    public Project Project { get; private set; }
    public ProjectVacancyStatus Status { get; private set; } = ProjectVacancyStatus.Open;

    public DateTime CreatedAt { get; private set; }
    
    public static Result<ProjectVacancy, Error> Create(ProjectId projectId, Title title, Description description)
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

    public void Close() => Status = ProjectVacancyStatus.Closed;
    public void Archive() => Status = ProjectVacancyStatus.Archived;
    public void Reopen() => Status = ProjectVacancyStatus.Open;
}