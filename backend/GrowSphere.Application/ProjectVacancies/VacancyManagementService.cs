using CSharpFunctionalExtensions;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain.Models.ProjectVacancyModel;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain;

namespace GrowSphere.Application.ProjectVacancies;

public class VacancyManagementService
{
    private readonly IProjectVacancyRepository _vacancyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISkillRepository _skillRepository;

    public VacancyManagementService(
        IProjectVacancyRepository vacancyRepository,
        IUnitOfWork unitOfWork,
        ISkillRepository skillRepository)
    {
        _vacancyRepository = vacancyRepository;
        _unitOfWork = unitOfWork;
        _skillRepository = skillRepository;
    }

    public async Task<Result<ProjectVacancy, Error>> CreateVacancyAsync(
        Guid projectId, string title, string description, List<Guid> skillIds, CancellationToken cancellationToken = default)
    {
        var vacancyResult = await CreateVacancyEntityAsync(projectId, title, description, cancellationToken);
        if (vacancyResult.IsFailure)
            return vacancyResult.Error;

        var vacancy = vacancyResult.Value;
        await AssignSkillsToVacancyAsync(vacancy, skillIds, cancellationToken);
        await SaveVacancyAsync(vacancy, cancellationToken);
        
        return vacancy;
    }

    public async Task<List<ProjectVacancy>> GetAllByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _vacancyRepository.GetByProjectIdAsync(projectId, cancellationToken);
    }

    public async Task<ProjectVacancy?> GetByIdAsync(Guid vacancyId, CancellationToken cancellationToken = default)
    {
        return await _vacancyRepository.GetByIdAsync(vacancyId, cancellationToken);
    }

    public async Task<ProjectVacancy?> GetByIdWithDetailsAsync(Guid vacancyId, CancellationToken cancellationToken = default)
    {
        return await _vacancyRepository.GetByIdWithDetailsAsync(vacancyId, cancellationToken);
    }

    // Приватные методы

    private async Task<Result<ProjectVacancy, Error>> CreateVacancyEntityAsync(
        Guid projectId, string title, string description, CancellationToken cancellationToken)
    {
        var titleResult = Title.Create(title);
        var descriptionResult = Description.Create(description);
        
        var vacancyResult = ProjectVacancy.Create(
            ProjectId.Create(projectId),
            titleResult.Value,
            descriptionResult.Value);
        
        if (vacancyResult.IsFailure)
            return vacancyResult.Error;

        return vacancyResult.Value;
    }

    private async Task AssignSkillsToVacancyAsync(ProjectVacancy vacancy, List<Guid> skillIds, CancellationToken cancellationToken)
    {
        var skills = await _skillRepository.GetByIds(skillIds, cancellationToken);
        if (skills.Count != skillIds.Count)
            throw new InvalidOperationException("Не все навыки найдены");
        
        ((List<Skill>)vacancy.Skills).AddRange(skills);
    }

    private async Task SaveVacancyAsync(ProjectVacancy vacancy, CancellationToken cancellationToken)
    {
        await _vacancyRepository.AddAsync(vacancy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
} 