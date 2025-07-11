using GrowSphere.Application.Users;
using GrowSphere.Infrastructure.Repositories;
using GrowSphere.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GrowSphere.Application.DTOs;

namespace GrowSphere.Web.Controllers.User;

[ApiController]
[Route("api/user")]
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;
    private readonly UserRepository _userRepository;

    public AdminController(AdminService adminService, UserRepository userRepository)
    {
        _adminService = adminService;
        _userRepository = userRepository;
    }

    [Authorize]
    [HttpPost("{userId}/set-admin")]
    public async Task<IActionResult> SetUserAsAdmin(Guid userId, CancellationToken cancellationToken)
    {
        // Здесь можно добавить проверку на IsAdmin для текущего пользователя
        var result = await _adminService.SetUserAsAdmin(userId, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        return Ok();
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAll(cancellationToken);
        if(users.IsFailure)
            return users.Error.ToResponse();
        var userDtos = users.Value.Select(u => new UserProfileDto(
            u.Id.Value,
            u.Name.Value,
            u.Email.Value,
            u.Bio.Value,
            u.ProfilePicture?.Path ?? string.Empty,
            u.IsAdmin,
            u.Skills.Select(s => new SkillDto(s.Skill.Id.Value, s.Skill.Title.Value)).ToList()
        ));
        return Ok(userDtos);
    }
} 