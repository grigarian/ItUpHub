using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain.Models;
using GrowSphere.Domain.Models.UserModel;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.ProjectModel;

namespace GrowSphere.Application.Projects;

public class JoinRequestService
{
    private readonly IJoinRequestRepository _joinRequestRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly INotificationService _notificationService;
    private readonly ICurrentUserService _currentUserService;

    public JoinRequestService(
        IJoinRequestRepository joinRequestRepository,
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        INotificationService notificationService,
        ICurrentUserService currentUserService)
    {
        _joinRequestRepository = joinRequestRepository;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<JoinRequest, Error>> SendJoinRequest(Guid projectId, string message, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var user = await _userRepository.GetById(userId, cancellationToken);
        if (user.IsFailure)
            return Errors.General.NotFound(userId);

        var project = await _projectRepository.GetById(projectId, cancellationToken);
        if (project.IsFailure)
            return project.Error;

        var alreadyRequested = await _joinRequestRepository.ExistsAsync(projectId, userId, cancellationToken);
        if (alreadyRequested)
            return Errors.General.ValueIsInvalid("Project already requested");

        var requestResult = JoinRequest.Create(projectId, userId, message);
        if (requestResult.IsFailure)
            return requestResult.Error;

        await _joinRequestRepository.AddAsync(requestResult.Value, cancellationToken);
        await _joinRequestRepository.SaveChangesAsync(cancellationToken);
        
        await _notificationService.NotifyProjectManagersAsync(
            projectId,
            $"Новая заявка на вступление в проект: {project.Value.Title}",
            cancellationToken
        );

        return requestResult.Value;
    }

    public async Task<Result<List<JoinRequestDto>, Error>> GetJoinRequestsForProject(Guid projectId, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetById(projectId, cancellationToken);
        if (project.IsFailure)
            return project.Error;

        var currentUserId = _currentUserService.UserId;
        
        
        var members = await _projectRepository.GetMembers(project.Value.Id, cancellationToken);

        var isProjectManager = members.Value.Any(p => p.UserId == currentUserId && p.Role == MemberRole.ProjectManager.ToString());

        if(!isProjectManager)
            return Errors.General.ValueIsInvalid();
        
        
        var requests = await _joinRequestRepository.GetByProjectId(projectId, cancellationToken);
     
        
        
        var pendingRequests = requests
            .Where(r => r.Status == JoinRequestStatus.Pending)
            .ToList();

        var result = new List<JoinRequestDto>();

        foreach (var r in pendingRequests)
        {
            var userResult = await _userRepository.GetById(r.UserId, cancellationToken);
            if (userResult.IsSuccess)
            {
                result.Add(new JoinRequestDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = userResult.Value.Name.Value,
                    Message = r.Message,
                    Status = r.Status.ToString(),
                    CreatedAt = r.CreatedAt
                });
            }
        }

        return result;
    }

    public async Task<Result<Guid, Error>> ApproveJoinRequest(Guid requestId, MemberRole role,  CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        
        var request = await _joinRequestRepository.GetById(requestId, cancellationToken);
        if (request is null)
            return Errors.General.NotFound(requestId);

        var project = await _projectRepository.GetById(request.ProjectId, cancellationToken);
        if (project.IsFailure)
            return project.Error;

        var members = await _projectRepository.GetMembers(project.Value.Id, cancellationToken);

        var isProjectManager = members.Value
            .Any(p => p.UserId == currentUserId && p.Role == MemberRole.ProjectManager.ToString());

        if(!isProjectManager)
            return Errors.General.ValueIsInvalid();

        request.Approve();
        await _projectRepository.AddMember(project.Value.Id, request.UserId, role ,cancellationToken);
        await _joinRequestRepository.UpdateAsync(request, cancellationToken);
        await _joinRequestRepository.SaveChangesAsync(cancellationToken);

        await _notificationService.SendNotificationAsync(
            UserId.Create(request.UserId),
            $"Вы приняты в проект: {project.Value.Title.Value}",
            cancellationToken);

        return requestId;
    }

    public async Task<Result<Guid, Error>> RejectJoinRequest(Guid requestId, string reason, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        
        var request = await _joinRequestRepository.GetById(requestId, cancellationToken);
        if (request is null)
            return Errors.General.NotFound(requestId);

        var project = await _projectRepository.GetById(request.ProjectId, cancellationToken);
        if (project.IsFailure)
            return project.Error;

        var members = await _projectRepository.GetMembers(project.Value.Id, cancellationToken);

        var isProjectManager = members.Value
            .Any(p => p.UserId == currentUserId && p.Role == MemberRole.ProjectManager.ToString());

        if(!isProjectManager)
            return Errors.General.ValueIsInvalid();

        request.Reject(reason);
        await _joinRequestRepository.UpdateAsync(request, cancellationToken);
        await _joinRequestRepository.SaveChangesAsync(cancellationToken);

        await _notificationService.SendNotificationAsync(
            UserId.Create(request.UserId),
            $"Ваша заявка в проект \"{project.Value.Title.Value}\" отклонена. Причина: {reason}",
            cancellationToken);

        return requestId;
    }
}