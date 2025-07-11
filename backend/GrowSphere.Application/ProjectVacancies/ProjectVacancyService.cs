using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.ProjectVacancyModel;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.UserModel;
using GrowSphere.Application.Mappers;

namespace GrowSphere.Application.ProjectVacancies;

public class ProjectVacancyService
{
    private readonly IProjectVacancyRepository _vacancyRepository;
    private readonly IVacancyApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    private readonly IProjectRepository _projectRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IUserRepository _userRepository;

    public ProjectVacancyService(
        IProjectVacancyRepository vacancyRepository,
        IVacancyApplicationRepository applicationRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService,
        IProjectRepository projectRepository,
        ISkillRepository skillRepository,
        IUserRepository userRepository)
    {
        _vacancyRepository = vacancyRepository;
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _projectRepository = projectRepository;
        _skillRepository = skillRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<ProjectVacancy, Error>> CreateVacancyAsync(
        Guid projectId, string title, string description, List<Guid> skillIds, CancellationToken cancellationToken = default)
    {
        var titleResult = Title.Create(title);
        var descriptionResult = Description.Create(description);
        
        
        var vacancyResult = ProjectVacancy.Create(
            ProjectId.Create(projectId),
            titleResult.Value,
            descriptionResult.Value);
        if (vacancyResult.IsFailure)
            return vacancyResult.Error;

        var vacancy = vacancyResult.Value;
        foreach (var skillId in skillIds)
        {
            var skillIdResult = SkillId.Create(skillId);
            var skillResult = ProjectVacancySkill.Create(vacancy.Id, skillIdResult);
            if (skillResult.IsFailure)
                return skillResult.Error;

            vacancy.AddSkill(skillResult.Value.SkillId);
        }
        
        await _vacancyRepository.AddAsync(vacancy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        

        return vacancy;
        
        
    }

    public async Task<Result<VacancyApplicationDto, Error>> GetVacancyAsync(
        Guid vacancyId,
        CancellationToken cancellationToken = default)
    {
        var application = await _applicationRepository.GetByIdAsync(vacancyId, cancellationToken);
        var user = await _userRepository.GetById(application.UserId.Value, cancellationToken);
        return VacancyApplicationMapper.ToVacancyApplicationDto(application, user.Value.Name.Value, application.ManagerComment);
    }

    public async Task<Result<VacancyApplicationDto, Error>> ApplyToVacancyAsync(
        Guid vacancyId, Guid userId, string message, CancellationToken cancellationToken = default)
    {
        var applicationResult = VacancyApplication.Create(vacancyId, userId, message);
        if (applicationResult.IsFailure)
            return applicationResult.Error;
        var application = applicationResult.Value;
        var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId, cancellationToken);
        var project = await _projectRepository.GetById(vacancy.ProjectId, cancellationToken);
        var projectManager = _projectRepository
            .GetMembers(project.Value.Id, cancellationToken).Result.Value
            .FirstOrDefault(x => x.Role == MemberRole.ProjectManager);
        await _applicationRepository.AddAsync(application, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _notificationService.SendNotificationAsync(
            UserId.Create(projectManager.UserId),
            $"Новая заявка в проект: {project.Value.Title}",
            cancellationToken);
        var user = await _userRepository.GetById(application.UserId.Value, cancellationToken);
        return VacancyApplicationMapper.ToVacancyApplicationDto(application, user.Value.Name.Value, application.ManagerComment);
    }

    public async Task<List<VacancyApplicationDto>> GetApplicationsForVacancyAsync(Guid vacancyId, CancellationToken cancellationToken = default)
    {
        var applications = await _applicationRepository.GetByVacancyIdAsync(vacancyId, cancellationToken);
        var pendingApplications = applications
            .Where(a => a.Status == VacancyApplicationStatus.Pending)
            .ToList();
        var result = new List<VacancyApplicationDto>();
        foreach (var a in pendingApplications)
        {
            var user = await _userRepository.GetById(a.UserId.Value, cancellationToken);
            result.Add(VacancyApplicationMapper.ToVacancyApplicationDto(a, user.Value.Name.Value, a.ManagerComment));
        }
        return result;
    }

    public async Task<List<ProjectVacancyDto>> GetAllByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var vacancies = await _vacancyRepository.GetAllAsync(cancellationToken);
        var projectVacancies = vacancies
            .Where(x => x.ProjectId == projectId )
            .ToList();
        var result = new List<ProjectVacancyDto>();
        foreach (var vacancy in projectVacancies)
        {
            var applications = await _applicationRepository.GetByVacancyIdAsync(vacancy.Id, cancellationToken);
            var pendingApplications = applications
                .Where(a => a.Status == VacancyApplicationStatus.Pending)
                .ToList();
            var skills = await _skillRepository.GetSkillsForVacancyAsync(vacancy.Id, cancellationToken);
            result.Add(ProjectVacancyMapper.ToProjectVacancyDto(
                vacancy,
                skills,
                pendingApplications
            ));
        }
        return result;
    }

    public async Task<Result> ApproveApplicationAsync(Guid projectId, Guid applicationId, string? comment, MemberRole role ,CancellationToken cancellationToken = default)
    {
        var application = await _applicationRepository.GetByIdAsync(applicationId, cancellationToken);
        if (application is null)
            return Result.Failure("Заявка не найдена");

        var result = application.Approve(comment);
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _projectRepository.AddMember(projectId, application.UserId.Value, role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _notificationService.SendNotificationAsync(
            UserId.Create(application.UserId.Value),
            $"Ваша заявка в проект принята! Сообщение менеджера: {comment}",
            cancellationToken);
        return Result.Success();
    }

    public async Task<Result> RejectApplicationAsync(Guid applicationId, string? comment, CancellationToken cancellationToken = default)
    {
        var application = await _applicationRepository.GetByIdAsync(applicationId, cancellationToken);
        if (application is null)
            return Result.Failure("Заявка не найдена");

        var result = application.Reject(comment);
        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _notificationService.SendNotificationAsync(
            UserId.Create(application.UserId.Value),
            $"Ваша заявка в проект отклонена, по причине: {comment}",
            cancellationToken);
        return Result.Success();
    }
}