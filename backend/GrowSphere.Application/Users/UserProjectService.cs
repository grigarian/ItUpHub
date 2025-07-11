using CSharpFunctionalExtensions;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Application.Users;

public class UserProjectService
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserProjectService(
        IUserRepository userRepository,
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Project, Error>> AddProject(
        Guid userId,
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(userId, cancellationToken);
        if (user.IsFailure)
            return user.Error;
        var project = await _projectRepository.GetById(projectId, cancellationToken);
        if(project.IsFailure)
            return project.Error;
        var userProjects = await _projectRepository.GetAllByUserId(userId, cancellationToken);
        if (userProjects.Value.Any(s => s.Id == projectId))
        {
            return Errors.General.ValueIsInvalid("Project Id already exists");
        }
        user.Value.AddProject(project.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return project.Value;
    }

    public async Task<Result<Guid, Error>> RemoveProject(
        Guid userId,
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var userIdObj = UserId.Create(userId);
        var projectIdObj = ProjectId.Create(projectId);
        if (userIdObj.Value == Guid.Empty || projectIdObj.Value == Guid.Empty)
            return Errors.General.NotFound();
        return await _userRepository.RemoveProject(userIdObj, projectIdObj, cancellationToken);
    }
} 