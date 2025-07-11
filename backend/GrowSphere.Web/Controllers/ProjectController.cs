using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Application.Projects;
using GrowSphere.Application.Users;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GrowSphere.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    public ProjectController(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] ProjectService service,
        [FromServices] UserProjectService userService,
        [FromServices] ICurrentUserService currentUserService,
        [FromBody] CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.Create(request, cancellationToken);

        var userid = currentUserService.UserId;

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        await userService.AddProject(userid.Value, result.Value, cancellationToken);
        
        Log.Information
        ("Project {Title} successfully create on {Time}",
            request.Title,
            DateTime.Now);

        return result.Value;
    }

    [HttpGet("{id}/get")]
    public async Task<ActionResult<Project>> GetById(
        Guid id,
        [FromServices] ProjectService service,
        CancellationToken cancellationToken)
    {
        var project = await service.GetById(id, cancellationToken);
        if (project.IsFailure)
            return project.Error.ToResponse();
        
        return Ok(project.Value);
    }

    [Authorize]
    [HttpPut("{id:guid}/update")]
    public async Task<ActionResult> Update(
        Guid id,
        [FromBody] UpdateProjectRequest request,
        [FromServices] ProjectService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Update(id, request, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return NoContent();
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Project>>> All(
        [FromServices] ProjectService service,
        CancellationToken cancellationToken)
    {
        var projects = await service.GetAll(cancellationToken);
        
        if(projects.IsFailure)
            return projects.Error.ToResponse();
        
        return Ok(projects.Value);
    }

    [HttpGet("all-with-categories")]
    public async Task<ActionResult<IEnumerable<ProjectWithCategoryDto>>> AllWithCategories(
        [FromServices] ProjectService service,
        CancellationToken cancellationToken)
    {
        var projects = await service.GetAllWithCategories(cancellationToken);
        
        if(projects.IsFailure)
            return projects.Error.ToResponse();
        
        return Ok(projects.Value);
    }
    
    [Authorize]
    [HttpGet("all/{userId:guid}")]
    public async Task<ActionResult<IEnumerable<Project>>> AllByUserId(
        Guid userId,
        [FromServices] ProjectService service,
        CancellationToken cancellationToken)
    {
        var projects = await service.GetByUserId(userId, cancellationToken);
        
        if(projects.IsFailure)
            return projects.Error.ToResponse();
        
        return Ok(projects.Value);
    }
    
    [Authorize]
    [HttpGet("all-titles/{userId:guid}")]
    public async Task<ActionResult<IEnumerable<ProjectListItemDto>>> AllTitlesByUserId(
        Guid userId,
        [FromServices] ProjectService service,
        CancellationToken cancellationToken)
    {
        var projects = await service.GetAllTitlesByUserId(userId, cancellationToken);
        
        if(projects.IsFailure)
            return projects.Error.ToResponse();
        
        return Ok(projects.Value);
    }

    [Authorize]
    [HttpGet("{projectId:guid}/members")]
    public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> Members(
        Guid projectId,
        [FromServices] ProjectService service,
        CancellationToken cancellationToken)
    {
        var members = await service.GetMembers(projectId, cancellationToken);
        if(members.IsFailure)
            return members.Error.ToResponse();
        
        return Ok(members.Value);
    }
}