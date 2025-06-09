using GrowSphere.Application.Issues;
using GrowSphere.Domain.Models.IssueModel;
using GrowSphere.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace GrowSphere.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssueController: ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] IssueService issueService,
        [FromBody] CreateIssueRequest request,
        CancellationToken cancellationToken)
    {
        var result =  await issueService.CreateAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromServices] IssueService issueService,
        [FromBody] UpdateIssueRequest request,
        CancellationToken cancellationToken)
    {
        var result = await issueService.UpdateAsync(id, request, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetByProjectGrouped(
        Guid projectId,
        [FromServices] IssueService issueService,
        CancellationToken cancellationToken)
    {
        var result = await issueService.GetProjectIssuesGroupedByStatusAsync(
            projectId, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        var response = result.Value.ToDictionary(
            g => g.Key.Value,
            g => g.Value.Select(issue => new
            {
                id = issue.Id.Value,
                title = issue.Title.Value,
                description = issue.Description.Value,
                assigner = issue.AssignerUserId.Value,
                assignedTo = issue.AssignedUserId?.Value,
                status = issue.Status.Value,
                completeDate = issue.CompleteDate,
                order = issue.Order
            }).ToList());

        return Ok(response);
    }
    
    [HttpPost("{issueId:guid}/assign")]
    public async Task<IActionResult> Assign(
        Guid issueId,
        [FromServices] IssueService issueService,
        AssignUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await issueService.AssignIssueToUserAsync(
            issueId,
            request,
            cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("{issueId:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid issueId,
        [FromServices] IssueService issueService,
        ChangeStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await issueService.ChangeIssueStatusAsync(
            issueId,
            request,
            cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result);
    }
    
    [HttpPost("project/{projectId:guid}/reorder")]
    public async Task<IActionResult> Reorder(
        Guid projectId,
        [FromServices] IssueService issueService,
        ReorderIssuesRequest request,
        CancellationToken cancellationToken)
    {
        var statusGroups = request.Columns.ToDictionary(
            col => IssueStatus.FromString(col.Status).Value,
            col => col.IssueIds.Select(id => IssueId.Create(id)).ToList()
        );

        var result = await issueService.ReorderIssuesAsync(
            projectId,
            statusGroups,
            cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
