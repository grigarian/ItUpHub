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
[Route("[controller]")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    public ProjectController(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] ProjectService service,
        [FromServices] UserService userService,
        [FromServices] CurrentUserService currentUserService,
        [FromBody] CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.Create(request, cancellationToken);

        var userid = currentUserService.UserId;

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        await userService.AddProject(userid, result.Value, cancellationToken);
        
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

    [Authorize]
    [HttpGet("/all")]
    public async Task<ActionResult<IEnumerable<Project>>> All(
        [FromServices] ProjectService service,
        CancellationToken cancellationToken)
    {
        var projects = await service.GetAll(cancellationToken);
        
        if(projects.IsFailure)
            return projects.Error.ToResponse();
        
        return Ok(projects.Value);
    }
    
    [Authorize]
    [HttpGet("/all/{userId:guid}")]
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
    [HttpGet("/all-titles/{userId:guid}")]
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
}