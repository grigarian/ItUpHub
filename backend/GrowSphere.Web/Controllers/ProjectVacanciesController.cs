using GrowSphere.Application.DTOs;
using GrowSphere.Application.ProjectVacancies;
using GrowSphere.Application.Users;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GrowSphere.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectVacanciesController: ControllerBase
{
    [HttpGet("{projectId:guid}")]
    public async Task<IActionResult> GetAll(
        Guid projectId,
        [FromServices] ProjectVacancyService projectVacancyService,
        CancellationToken cancellationToken)
    {
        var result = await projectVacancyService.GetAllByProjectIdAsync(projectId, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("{projectId:guid}")]
    public async Task<IActionResult> Create(
        Guid projectId,
        [FromServices] ProjectVacancyService projectVacancyService,
        [FromBody] CreateVacancyRequest request,
        CancellationToken cancellationToken)
    {
        var result = await projectVacancyService.CreateVacancyAsync(projectId, request.Title, request.Description, request.SkillIds, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error.Message);
    }
    
    [HttpPost("{vacancyId:guid}/apply")]
    public async Task<IActionResult> Apply(
        Guid vacancyId,
        [FromBody] ApplyToVacancyRequest request,
        [FromServices] ProjectVacancyService projectVacancyService,
        [FromServices] CurrentUserService currentUserService,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId; // допустим, ты достаёшь его из токена
        var result = await projectVacancyService.ApplyToVacancyAsync(vacancyId, userId, request.Message, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error.Message);
    }
    
    [HttpGet("vacancy/{vacancyId:guid}")]
    public async Task<IActionResult> GetVacancy(
        Guid vacancyId,
        [FromServices] ProjectVacancyService projectVacancyService,
        CancellationToken cancellationToken)
    {
        var result = await projectVacancyService.GetVacancyAsync(vacancyId, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error.Message);
    }
    
    [HttpGet("vacancy/{vacancyId:guid}/applications")]
    public async Task<IActionResult> GetApplications(
        Guid vacancyId,
        [FromServices] ProjectVacancyService projectVacancyService,
        CancellationToken cancellationToken)
    {
        var result = await projectVacancyService.GetApplicationsForVacancyAsync(vacancyId, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("{projectId:guid}/application/{applicationId:guid}/approve")]
    public async Task<IActionResult> Approve(
        Guid projectId,
        Guid applicationId,
        [FromServices] ProjectVacancyService projectVacancyService,
        [FromBody] ApproveApplicationRequest request,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<MemberRole>(request.Role, ignoreCase: true, out var parsedRole))
            return BadRequest("Invalid role");
        
        await projectVacancyService.ApproveApplicationAsync(projectId,applicationId, request.Message, parsedRole,cancellationToken);

        return Ok();
    }
    
    [HttpPost("application/{applicationId:guid}/reject")]
    public async Task<IActionResult> Reject(
        Guid applicationId,
        [FromServices] ProjectVacancyService projectVacancyService,
        [FromBody] RejectApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await projectVacancyService.RejectApplicationAsync(applicationId, request.Message, cancellationToken);

        return Ok();
    }
}