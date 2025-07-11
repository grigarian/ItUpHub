using GrowSphere.Application.Users;
using GrowSphere.Application.Skills;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Infrastructure.Repositories;
using GrowSphere.Extentions;

namespace GrowSphere.Web.Controllers.User;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly SkillService _skillService;
    private readonly ProfileService _userService;

    public ProfileController(UserRepository userRepository, ICurrentUserService currentUserService,  SkillService skillService, ProfileService userService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _skillService = skillService;
        _userService = userService;
    }

    [Authorize]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == null || userId == Guid.Empty)
            return Unauthorized(new { message = "Пользователь не авторизован" });
        
        var userResult = await _userRepository.GetById(userId.Value, cancellationToken);
        if (userResult.IsFailure)
            return userResult.Error.ToResponse();
        var user = userResult.Value;

        var userDto = new UserProfileDto(
            user.Id.Value,
            user.Name.Value,
            user.Email.Value,
            user.Bio.Value,
            user.ProfilePicture?.Path ?? string.Empty,
            user.IsAdmin,
            user.Skills.Select(s => new SkillDto(s.SkillId.Value, s.Skill.Title.Value)).ToList());
        return Ok(userDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetById(id, cancellationToken);
        if (userResult.IsFailure)
            return userResult.Error.ToResponse();
        var user = userResult.Value;
        var skills = await _skillService.GetByUserId(user.Id, cancellationToken);
        if(skills.IsFailure)
            return skills.Error.ToResponse();
        var userDto = new UserProfileDto(
            user.Id.Value,
            user.Name.Value,
            user.Email.Value,
            user.Bio.Value,
            user.ProfilePicture.Path,
            user.IsAdmin,
            user.Skills.Select(s => new SkillDto(s.SkillId, s.Skill.Title.Value)).ToList());
        return Ok(userDto);
    }

    [Authorize]
    [HttpPut("bio")]
    public async Task<IActionResult> UpdateBio([FromBody] UserProfileRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateUserBio(request, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        return Ok($"{result.Value}");
    }

    [Authorize]
    [HttpPost("profile-image")]
    public async Task<IActionResult> UploadProfileImage(IFormFile file, CancellationToken cancellationToken)
    {
        var result = await _userService.UploadImage(file, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(new { picturePath = result.Value.Path });
    }
} 