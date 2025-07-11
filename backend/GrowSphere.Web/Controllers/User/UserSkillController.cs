using GrowSphere.Application.Users;
using GrowSphere.Infrastructure.Repositories;
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
public class UserSkillController : ControllerBase
{
    private readonly UserSkillService _userService;
    private readonly ISkillRepository _skillRepository;
    private readonly ICurrentUserService _currentUserService;

    public UserSkillController(UserSkillService userService, ISkillRepository skillRepository, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _skillRepository = skillRepository;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost("{userId}/skill")]
    public async Task<IActionResult> AddSkill(Guid userId, [FromBody] AddSkillToUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.AddSkill(userId, request, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("{userId:guid}/skills/{skillId:guid}")]
    public async Task<IActionResult> RemoveSkill(Guid userId, Guid skillId, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId != userId)
            return Forbid();
        var result = await _userService.RemoveSkill(userId, skillId, _skillRepository, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }
} 