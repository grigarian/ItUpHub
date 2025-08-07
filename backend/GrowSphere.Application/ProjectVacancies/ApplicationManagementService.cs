 using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.ProjectVacancyModel;
using GrowSphere.Domain.Models.UserModel;
using GrowSphere.Application.Mappers;
using GrowSphere.Domain;

namespace GrowSphere.Application.ProjectVacancies;

public class ApplicationManagementService
{
    private readonly IVacancyApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly INotificationService _notificationService;
    private readonly IProjectVacancyRepository _vacancyRepository;

    public ApplicationManagementService(
        IVacancyApplicationRepository applicationRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IProjectRepository projectRepository,
        INotificationService notificationService,
        IProjectVacancyRepository vacancyRepository)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _notificationService = notificationService;
        _vacancyRepository = vacancyRepository;
    }

    public async Task<Result<VacancyApplicationDto, Error>> ApplyToVacancyAsync(
        Guid vacancyId, Guid userId, string message, CancellationToken cancellationToken = default)
    {
        var applicationResult = await CreateApplicationAsync(vacancyId, userId, message, cancellationToken);
        if (applicationResult.IsFailure)
            return applicationResult.Error;

        var application = applicationResult.Value;
        await SaveApplicationAsync(application, cancellationToken);
        await NotifyProjectManagerAboutApplicationAsync(application, cancellationToken);
        
        var user = await _userRepository.GetById(application.UserId.Value, cancellationToken);
        return VacancyApplicationMapper.ToVacancyApplicationDto(application, user.Value.Name.Value, application.ManagerComment);
    }

    public async Task<Result<VacancyApplicationDto, Error>> GetVacancyAsync(
        Guid vacancyId, CancellationToken cancellationToken = default)
    {
        var application = await _applicationRepository.GetByIdAsync(vacancyId, cancellationToken);
        var user = await _userRepository.GetById(application.UserId.Value, cancellationToken);
        return VacancyApplicationMapper.ToVacancyApplicationDto(application, user.Value.Name.Value, application.ManagerComment);
    }

    public async Task<Result> ApproveApplicationAsync(Guid applicationId, string? comment, MemberRole role, CancellationToken cancellationToken = default)
    {
        var application = await GetApplicationByIdAsync(applicationId, cancellationToken);
        if (application is null)
            return Result.Failure("Заявка не найдена");

        var approveResult = await ApproveApplicationEntityAsync(application, comment, cancellationToken);
        if (approveResult.IsFailure)
            return approveResult;

        await AddUserToProjectAsync(application, role, cancellationToken);
        await NotifyUserAboutApprovalAsync(application, comment, cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result> RejectApplicationAsync(Guid applicationId, string? comment, CancellationToken cancellationToken = default)
    {
        var application = await GetApplicationByIdAsync(applicationId, cancellationToken);
        if (application is null)
            return Result.Failure("Заявка не найдена");

        var rejectResult = await RejectApplicationEntityAsync(application, comment, cancellationToken);
        if (rejectResult.IsFailure)
            return rejectResult;

        await NotifyUserAboutRejectionAsync(application, comment, cancellationToken);
        return Result.Success();
    }

    // Приватные методы

    private async Task<Result<VacancyApplication, Error>> CreateApplicationAsync(
        Guid vacancyId, Guid userId, string message, CancellationToken cancellationToken)
    {
        var applicationResult = VacancyApplication.Create(vacancyId, userId, message);
        if (applicationResult.IsFailure)
            return applicationResult.Error;
        
        return applicationResult.Value;
    }

    private async Task SaveApplicationAsync(VacancyApplication application, CancellationToken cancellationToken)
    {
        await _applicationRepository.AddAsync(application, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task NotifyProjectManagerAboutApplicationAsync(VacancyApplication application, CancellationToken cancellationToken)
    {
        var vacancy = await GetVacancyByIdAsync(application.ProjectVacancyId, cancellationToken);
        var project = await _projectRepository.GetById(vacancy.ProjectId, cancellationToken);
        var projectManager = await GetProjectManagerAsync(project.Value.Id, cancellationToken);
        
        if (projectManager != null)
        {
            await _notificationService.SendNotificationAsync(
                UserId.Create(projectManager.UserId),
                $"Новая заявка в проект: {project.Value.Title}",
                cancellationToken);
        }
    }

    private async Task<ProjectMember?> GetProjectManagerAsync(ProjectId projectId, CancellationToken cancellationToken)
    {
        var members = await _projectRepository.GetMembers(projectId, cancellationToken);
        return members.Value.FirstOrDefault(x => x.Role == MemberRole.ProjectManager);
    }

    private async Task<VacancyApplication?> GetApplicationByIdAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        return await _applicationRepository.GetByIdAsync(applicationId, cancellationToken);
    }

    private async Task<Result> ApproveApplicationEntityAsync(VacancyApplication application, string? comment, CancellationToken cancellationToken)
    {
        var result = application.Approve(comment);
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private async Task AddUserToProjectAsync(VacancyApplication application, MemberRole role, CancellationToken cancellationToken)
    {
        await _projectRepository.AddMember(application.ProjectVacancy.ProjectId, application.UserId.Value, role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task NotifyUserAboutApprovalAsync(VacancyApplication application, string? comment, CancellationToken cancellationToken)
    {
        await _notificationService.SendNotificationAsync(
            UserId.Create(application.UserId.Value),
            $"Ваша заявка в проект принята! Сообщение менеджера: {comment}",
            cancellationToken);
    }

    private async Task<Result> RejectApplicationEntityAsync(VacancyApplication application, string? comment, CancellationToken cancellationToken)
    {
        var result = application.Reject(comment);
        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private async Task NotifyUserAboutRejectionAsync(VacancyApplication application, string? comment, CancellationToken cancellationToken)
    {
        await _notificationService.SendNotificationAsync(
            UserId.Create(application.UserId.Value),
            $"Ваша заявка в проект отклонена, по причине: {comment}",
            cancellationToken);
    }

    private async Task<ProjectVacancy?> GetVacancyByIdAsync(Guid vacancyId, CancellationToken cancellationToken)
    {
        return await _vacancyRepository.GetByIdAsync(vacancyId, cancellationToken);
    }
}