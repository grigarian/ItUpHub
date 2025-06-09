using CSharpFunctionalExtensions;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.IssueModel;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Application.Issues;

public class IssueService
{
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;

    public IssueService(
        IIssueRepository issueRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IProjectRepository projectRepository)
    {
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Result<Guid, Error>> CreateAsync(
        CreateIssueRequest request,
        CancellationToken cancellationToken)
    {
        var id = IssueId.NewId();
        
        var title = Title.Create(request.Title);
        if (title.IsFailure)
            return title.Error;
        
        var description = Description.Create(request.Description);
        if(description.IsFailure)
            return description.Error;
        
        var projectId= ProjectId.Create(request.ProjectId);
        if(projectId.Value == Guid.Empty)
            return Errors.General.NotFound(request.ProjectId);

        var issueStatus = IssueStatus.FromString(IssueStatus.Backlog.Value);
        if(issueStatus.IsFailure)
            return issueStatus.Error;
        
        var assignedUserId = UserId.Create(request.AssignedUserId);
        if(assignedUserId.Value == Guid.Empty)
            return Errors.General.NotFound(request.AssignedUserId);
        
        var assignedUser = await _userRepository.GetById(assignedUserId.Value, cancellationToken);
        if(assignedUser.IsFailure)
            return assignedUser.Error;
        
        var assignerUserId = UserId.Create(request.AssignerUserId);
        if(assignedUserId.Value == Guid.Empty)
            return Errors.General.NotFound(request.AssignerUserId);
        
        var assignerUser = await _userRepository.GetById(assignerUserId.Value, cancellationToken);
        if(assignerUser.IsFailure)
            return assignerUser.Error;
        
        var project = await _projectRepository.GetById(projectId.Value, cancellationToken);
        if(project.IsFailure)
            return project.Error;
        
        var nextOrder = (await _issueRepository.GetByProjectIdAsync(projectId, cancellationToken)).Count;
        
        
        var issue = Issue.Create(
            id,
            title.Value,
            description.Value,
            request.CompleteDate.ToUniversalTime(),
            issueStatus.Value,
            assignedUserId,
            assignerUserId,
            Picture.Empty(),
            order: nextOrder);

        issue.Value.AssignToUser(assignedUser.Value);
        issue.Value.SetProject(project.Value);
        
        await _issueRepository.AddAsync(issue.Value, cancellationToken);

        return issue.Value.Id.Value;
    }

    public async Task<Result<Result, Error>> UpdateAsync(
        Guid issueId,
        UpdateIssueRequest request,
        CancellationToken cancellationToken)
    {
        var issueIdResult = IssueId.Create(issueId);
        var issue = await _issueRepository.GetByIdAsync(issueIdResult, cancellationToken);
        if(issue is null)
            return Errors.General.NotFound(issueIdResult);
        
        var title = Title.Create(request.Title);
        if(title.IsFailure)
            return title.Error;
        
        var description = Description.Create(request.Description);
        if(description.IsFailure)
            return description.Error;

        if (request.CompleteDate.ToUniversalTime() <= DateTime.UtcNow)
            return Errors.General.ValueIsInvalid("complete.date");
        
        var assignedUserId = UserId.Create(request.AssignedUserId);
        if(assignedUserId.Value == Guid.Empty)
            return Errors.General.NotFound(request.AssignedUserId);
        
        var assignedUser = await _userRepository.GetById(assignedUserId.Value, cancellationToken);
        if(assignedUser.IsFailure)
            return assignedUser.Error;
        
        issue.AssignToUser(assignedUser.Value);
        issue.Update(
            title.Value,
            description.Value,
            request.CompleteDate.ToUniversalTime(),
            assignedUserId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result<IEnumerable<Issue>, Error>> GetByProjectId(Guid projectId,
        CancellationToken cancellationToken)
    {
        var result = await _issueRepository.GetByProjectIdAsync(projectId, cancellationToken);

        return result;
    }
    
    public async Task<Result<Result, Error>> AssignIssueToUserAsync(
        Guid issueId,
        AssignUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var issueIdResult = IssueId.Create(issueId);
        var userId = UserId.Create(request.UserId);
        
        var issue = await _issueRepository.GetByIdAsync(issueIdResult, cancellationToken);
        if (issue is null)
            return Errors.General.NotFound(issueIdResult.Value);

        var user = await _userRepository.GetById(userId, cancellationToken);
        if (user.IsFailure)
            return Errors.General.NotFound(userId.Value);

        issue.AssignToUser(user.Value);
        _issueRepository.Update(issue);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    
    public async Task<Result<Result, Error>> ChangeIssueStatusAsync(
        Guid issueId,
        ChangeStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var issueIdResult = IssueId.Create(issueId);
        
        var issue = await _issueRepository.GetByIdAsync(issueIdResult, cancellationToken);
        if (issue is null)
            return Errors.General.NotFound(issueIdResult.Value);

        var status = IssueStatus.FromString(request.Status);
        
        issue.UpdateStatus(status.Value);
        _issueRepository.Update(issue);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    
    public async Task<Result<IReadOnlyDictionary<IssueStatus, List<Issue>>, Error>> 
        GetProjectIssuesGroupedByStatusAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        
        var project = await _projectRepository.GetById(projectId, cancellationToken);
        if (project.IsFailure)
            return project.Error;

        var issues = await _issueRepository.GetByProjectIdAsync(projectId, cancellationToken);

        var grouped = issues
            .GroupBy(i => i.Status)
            .ToDictionary(g => g.Key, g => g.OrderBy(i => i.Order).ToList());

        return grouped;
    }
    
    public async Task<Result> ReorderIssuesAsync(Guid projectId,
        Dictionary<IssueStatus, List<IssueId>> columns,
        CancellationToken cancellationToken = default)
    {
        var issues = await _issueRepository.GetByProjectIdAsync(projectId, cancellationToken);

        foreach (var (status, issueIds) in columns)
        {
            for (int i = 0; i < issueIds.Count; i++)
            {
                var issue = issues.FirstOrDefault(x => x.Id.Value == issueIds[i]);
                if (issue is null) continue;

                issue.UpdateStatus(status);
                issue.UpdateOrder(i);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}