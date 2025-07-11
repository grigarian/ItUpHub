using GrowSphere.Application.Users;
using GrowSphere.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using GrowSphere.Application.Interfaces;

namespace GrowSphere.Web.Controllers.User;

[ApiController]
[Route("api/user")]
public class UserProjectController : ControllerBase
{
    private readonly UserProjectService _userService;
    private readonly ICurrentUserService _currentUserService;

    public UserProjectController(UserProjectService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost("{userId}/project")]
    public async Task<IActionResult> AddProject(Guid userId, Guid projectId, CancellationToken cancellationToken)
    {
        var result = await _userService.AddProject(userId, projectId, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("{userId:guid}/projects/{projectId:guid}")]
    public async Task<IActionResult> RemoveProject(Guid userId, Guid projectId, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId != userId)
            return Forbid();
        var result = await _userService.RemoveProject(userId, projectId, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }
} 