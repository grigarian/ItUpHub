using GrowSphere.Application.DTOs;
using GrowSphere.Application.Projects;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace GrowSphere.Web.Controllers;

[ApiController]
[Route("join-request")]
public class JoinRequestController: ControllerBase
{

    [HttpPost("{projectId}")]
    public async Task<IActionResult> SendRequest(
        Guid projectId,
        [FromBody] SendJoinRequestDto dto,
        [FromServices] JoinRequestService service,
        CancellationToken cancellationToken)
    {
        var result = await service.SendJoinRequest(projectId, dto.Message, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpGet("{projectId:guid}/requests")]
    public async Task<IActionResult> GetRequests(
        Guid projectId,
        [FromServices] JoinRequestService service,
        CancellationToken cancellationToken)
    {
        var result = await service.GetJoinRequestsForProject(projectId, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("{requestId}/approve")]
    public async Task<IActionResult> Approve(
        Guid requestId,
        [FromServices] JoinRequestService service,
        [FromBody] ApproveJoinRequestDto dto,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<MemberRole>(dto.Role, ignoreCase: true, out var parsedRole))
            return BadRequest("Invalid role");

        var result = await service.ApproveJoinRequest(requestId, parsedRole, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error.Message);
    }

    [HttpPost("{requestId}/reject")]
    public async Task<IActionResult> Reject(
        Guid requestId,
        [FromServices] JoinRequestService service,
        [FromBody] RejectJoinRequestDto dto,
        CancellationToken cancellationToken)
    {
        var result = await service.RejectJoinRequest(requestId, dto.Reason, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error.Message);
    }
}