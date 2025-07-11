using System.Collections;
using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Application.Mappers;
using System.Linq;

namespace GrowSphere.Application.Projects;

public class ProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProjectService(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        ICategoryRepository categoryRepository)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Guid, Error>> Create(
        CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
            return Result.Failure<Guid, Error>(Errors.General.Unauthorized());
        
        var user = await _userRepository.GetById(userId.Value, cancellationToken);
        if (user.IsFailure)
            return user.Error;
        
        var projectId = ProjectId.NewId();
        
        var title = Title.Create(request.Title);
        if (title.IsFailure)
            return title.Error;
        
        var description = Description.Create(request.Description);
        if(description.IsFailure)
            return description.Error;

        var member = ProjectMember.Create(projectId,user.Value.Id, MemberRole.ProjectManager);
        if(member.IsFailure)
            return member.Error;
        
        member.Value.SetUser(user.Value);
        
        var category = await _categoryRepository.GetById(
            request.CategoryId,
            cancellationToken);
        if(category.IsFailure)
            return category.Error;
        
        var startDate = request.StartDate.ToUniversalTime();
        var endDate = request.EndDate?.ToUniversalTime();

        var project = Project.Create(
            projectId,
            title.Value,
            description.Value,
            ProjectStatus.InProgress,
            startDate,
            endDate,
            DateTime.UtcNow,
            DateTime.UtcNow);
        
        project.Value.AddCategory(category.Value);
        member.Value.SetProject(project.Value);
        project.Value.AddMember(member.Value);

        if (project.IsFailure)
            return project.Error;
        
        await _projectRepository.Add(project.Value, cancellationToken);

        return projectId.Value;

    }

    public async Task<Result<Result, Error>> Update(
        Guid projectId,
        UpdateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var projectResult = await _projectRepository.GetById(projectId, cancellationToken);
        if(projectResult.IsFailure)
            return projectResult.Error;
        
        var project = projectResult.Value;

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            var result = project.ChangeTitle(request.Title);
            if(result.IsFailure)
                return result.Error;
        }
        
        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            var result = project.ChangeDescription(request.Description);
            if(result.IsFailure)
                return result.Error;
        }
        
        if (!string.IsNullOrWhiteSpace(request.ProjectStatus))
        {
            var statusResult = ProjectStatus.Create(request.ProjectStatus);
            if(statusResult.IsFailure)
                return statusResult.Error;
            
            var statusUpdate = project.ChangeStatus(statusResult.Value);
            if(statusUpdate.IsFailure)
                return statusUpdate.Error;
        }

        if (request.EndDate.HasValue)
        {
            var result = project.ChangeEndDate(request.EndDate);
            if(result.IsFailure)
                return result.Error;
        }

        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetById(
                request.CategoryId.Value,
                cancellationToken);
            if(category.IsFailure)
                return category.Error;
            
            var result = project.ChangeCategory(category.Value);
            if(result.IsFailure)
                return result.Error;
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
        
    }

    public async Task<Result<IEnumerable<Project>, Error>> GetAll(CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAll(cancellationToken);
        
        if(projects.IsFailure)
            return projects.Error;
        
        return projects.Value.ToList();
    }

    public async Task<Result<IEnumerable<ProjectWithCategoryDto>, Error>> GetAllWithCategories(CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllWithCategories(cancellationToken);
        
        if(projects.IsFailure)
            return projects.Error;
        
        var dtos = projects.Value.Select(ProjectMapper.ToProjectWithCategoryDto).ToList();
        return dtos;
    }

    public async Task<Result<ProjectDto, Error>> GetById(Guid projectId, CancellationToken cancellationToken)
    {
        var projectResult = await _projectRepository.GetById(projectId, cancellationToken);
        
        if(projectResult.IsFailure)
            return projectResult.Error;
        
        var project = projectResult.Value;
        
        var members = await _projectRepository.GetMembers(ProjectId.Create(projectId), cancellationToken);
        if(members.IsFailure)
            return members.Error;
        
        string categoryName = "Без категории";
        if (project.CategoryId != null && project.CategoryId.Value != Guid.Empty)
        {
            var categoryResult = await _categoryRepository.GetById(project.CategoryId.Value, cancellationToken);
            if(categoryResult.IsSuccess)
            {
                categoryName = categoryResult.Value.Title.Value;
            }
        }
        
        return ProjectMapper.ToProjectDto(project, categoryName, members.Value);
    }
    
    public async Task<Result<IEnumerable<Project>, Error>> GetByUserId(Guid id,
        CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllByUserId(id ,cancellationToken);
        
        if(projects.IsFailure)
            return projects.Error;
        
        return projects.Value.ToList();
    }

    public async Task<Result<IEnumerable<ProjectListItemDto>, Error>> GetAllTitlesByUserId(Guid userId,
        CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllTitlesByUserId(userId, cancellationToken);
        
        if(projects.IsFailure)
            return projects.Error;
        
        var dtos = projects.Value.Select(ProjectMapper.ToProjectListItemDto).ToList();
        return dtos;
    }

    public async Task<Result<IEnumerable<ProjectMemberDto>, Error>> GetMembers(Guid projectId, CancellationToken cancellationToken)
    {
        var members = await _projectRepository.GetMembers(ProjectId.Create(projectId), cancellationToken);
        if(members.IsFailure)
            return members.Error;
        
        return members.Value.Select(ProjectMemberMapper.ToProjectMemberDto).ToList();
    }
    
}