using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
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
        [FromServices] VacancyManagementService vacancyManagementService,
        [FromServices] VacancyDataMappingService vacancyDataMappingService,
        CancellationToken cancellationToken)
    {
        var vacancies = await vacancyManagementService.GetAllByProjectIdAsync(projectId, cancellationToken);
        var result = vacancyDataMappingService.MapVacanciesToDto(vacancies);
        return Ok(result);
    }
    
    [HttpPost("{projectId:guid}")]
    public async Task<IActionResult> Create(
        Guid projectId,
        [FromServices] VacancyManagementService vacancyManagementService,
        [FromBody] CreateVacancyRequest request,
        CancellationToken cancellationToken)
    {
        var result = await vacancyManagementService.CreateVacancyAsync(projectId, request.Title, request.Description, request.SkillIds, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error.Message);
    }
    
    [HttpPost("{vacancyId:guid}/apply")]
    public async Task<IActionResult> Apply(
        Guid vacancyId,
        [FromBody] ApplyToVacancyRequest request,
        [FromServices] ApplicationManagementService applicationManagementService,
        [FromServices] ICurrentUserService currentUserService,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        var result = await applicationManagementService.ApplyToVacancyAsync(vacancyId, userId.Value, request.Message, cancellationToken);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error.Message);
    }
    
    [HttpGet("vacancy/{vacancyId:guid}")]
    public async Task<IActionResult> GetVacancy(
        Guid vacancyId,
        [FromServices] ApplicationManagementService applicationManagementService,
        CancellationToken cancellationToken)
    {
        var result = await applicationManagementService.GetVacancyAsync(vacancyId, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error.Message);
    }
    
    [HttpGet("vacancy/{vacancyId:guid}/applications")]
    public async Task<IActionResult> GetApplications(
        Guid vacancyId,
        [FromServices] VacancyManagementService vacancyManagementService,
        [FromServices] VacancyDataMappingService vacancyDataMappingService,
        CancellationToken cancellationToken)
    {
        var vacancy = await vacancyManagementService.GetByIdWithDetailsAsync(vacancyId, cancellationToken);
        if (vacancy == null)
            return Ok(new List<VacancyApplicationDto>());
        var result = vacancyDataMappingService.MapPendingApplicationsToDto(vacancy.VacancyApplications);
        return Ok(result);
    }
    
    [HttpPost("application/{applicationId:guid}/approve")]
    public async Task<IActionResult> Approve(
        Guid applicationId,
        [FromServices] ApplicationManagementService applicationManagementService,
        [FromBody] ApproveApplicationRequest request,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<MemberRole>(request.Role, ignoreCase: true, out var parsedRole))
            return BadRequest("Invalid role");
        await applicationManagementService.ApproveApplicationAsync(applicationId, request.Message, parsedRole, cancellationToken);
        return Ok();
    }
    
    [HttpPost("application/{applicationId:guid}/reject")]
    public async Task<IActionResult> Reject(
        Guid applicationId,
        [FromServices] ApplicationManagementService applicationManagementService,
        [FromBody] RejectApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await applicationManagementService.RejectApplicationAsync(applicationId, request.Message, cancellationToken);
        return Ok();
    }
}