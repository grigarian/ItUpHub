using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> Add(Project project, CancellationToken cancellationToken)
    {
        await _context.Projects.AddAsync(project, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);

        return project.Id.Value;
    }

    public async Task<Result<Project, Error>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var projectId = ProjectId.Create(id);
        
        var project = await _context.Projects
            .FirstOrDefaultAsync(u => u.Id == projectId, cancellationToken);

        if (project == null)
            return Errors.General.NotFound(id);
        
        return project;
    }
    
    public async Task<Result<IEnumerable<Project>, Error>> GetAll(CancellationToken cancellationToken)
    {
        var projects = await _context.Projects
            .Include(p => p.Category)
            .ToListAsync(cancellationToken);
        
        return projects;
    }

    public async Task<Result<IEnumerable<ProjectWithCategoryDto>, Error>> GetAllWithCategories(CancellationToken cancellationToken)
    {
        var projects = await _context.Projects
            .Include(p => p.Category)
            .Select(p => new ProjectWithCategoryDto(
                p.Id.Value,
                p.Title.Value,
                p.Description.Value,
                p.Category != null ? p.Category.Title.Value : null,
                p.Status.Value,
                p.StartDate,
                p.EndDate,
                p.CreationDate
            ))
            .ToListAsync(cancellationToken);
        
        return projects;
    }

    public async Task<Result<IEnumerable<ProjectListItemDto>, Error>> GetAllTitlesByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var projects = await _context.UserProjects
            .Where(up => up.UserId == userId)
            .Select(up => new ProjectListItemDto(
                up.Project.Id.Value,
                up.Project.Title.Value,
                up.Project.Description.Value,
                up.Project.Category != null ? up.Project.Category.Title.Value : "Без категории"
            ))
            .ToListAsync(cancellationToken);
        
        return projects;
    }

    public async Task<Result<IEnumerable<Project>, Error>> GetAllByUserId
    (
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        if(userId.Equals(Guid.Empty))
            return Errors.General.NotFound(userId);
        
        var projects = await _context.UserProjects
            .Where(up => up.UserId == userId)
            .Include(up => up.Project)
            .Select(up => up.Project)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return projects;
    }

    public async Task<Result<IEnumerable<ProjectMemberDto>, Error>> GetMembers(ProjectId projectId, CancellationToken  cancellationToken)
    {
        var memberDtos = await _context.ProjectMembers
            .Where(pm => pm.ProjectId == projectId)
            .Include(pm => pm.User)
            .Select(pm => new ProjectMemberDto
            {
                UserId = pm.UserId.Value,
                UserName = pm.User.Name.Value,
                Role = pm.Role.ToString(),
                PictureUrl = pm.User.ProfilePicture.Path
            })
            .ToListAsync(cancellationToken);

        return memberDtos;
    }

    

    public async Task<Result<ProjectMember, Error>> AddMember(Guid projectId, Guid userId, MemberRole role ,CancellationToken cancellationToken)
    {
        var project = await GetById(projectId, cancellationToken);
        
        var member = ProjectMember.Create(ProjectId.Create(projectId), UserId.Create(userId), role);
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        
        user.AddProject(project.Value);
        
        project.Value.AddMember(member.Value);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return member;
    }
    
}