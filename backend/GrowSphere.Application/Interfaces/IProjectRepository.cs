using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Core;
using GrowSphere.Domain.Models.ProjectModel;

namespace GrowSphere.Application.Interfaces;

public interface IProjectRepository
{
    Task<Guid> Add(Project project, CancellationToken cancellationToken);
    
    Task<Result<Project, Error>> GetById(Guid id, CancellationToken cancellationToken);
    
    Task<Result<IEnumerable<Project>, Error>> GetAllByUserId(Guid userId, CancellationToken cancellationToken);
    
    Task<Result<IEnumerable<Project>, Error>> GetAll(CancellationToken cancellationToken);
    
    Task<Result<IEnumerable<ProjectListItemDto>, Error>> GetAllTitlesByUserId(Guid userId, CancellationToken cancellationToken);

    Task<Result<IEnumerable<ProjectMemberDto>, Error>> GetMembers(ProjectId projectId,
        CancellationToken cancellationToken);
    
    Task<Result<ProjectMember, Error>> AddMember (Guid projectId, Guid userId,MemberRole role ,CancellationToken cancellationToken);
}