using GrowSphere.Application.Users;
using GrowSphere.Application.Skills;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using GrowSphere.Application.DTOs;
using GrowSphere.Infrastructure.Repositories;
using GrowSphere.Extentions;

namespace GrowSphere.Web.Controllers.User;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly UserRepository _userRepository;

    public AuthController(AuthService authService, UserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<ActionResult<Guid>> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.Register(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        Log.Information("User {UserName} successfully registered on {Time}", request.UserName, DateTime.Now);
        return result.Value;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var token = await _authService.Login(request, cancellationToken);
        if(token.IsFailure)
            return token.Error.ToResponse();
        var user = await _userRepository.GetByEmail(request.Email);
        if(user.IsFailure)
            return user.Error.ToResponse();
        var context = HttpContext;
        Log.Information("Setting cookie for user {Email} with token length {TokenLength}", request.Email, token.Value.Length);
        context.Response.Cookies.Append("tasty-cookies", token.Value, new CookieOptions {
            Secure = false,
            SameSite = SameSiteMode.Lax,
            HttpOnly = false,
            Path = "/",
            Expires = DateTime.UtcNow.AddDays(30)
        });
        Log.Information("Cookie set successfully for user {Email}", request.Email);
        Log.Information("User {UserName} successfully login on {Time}", request.Email, DateTime.Now);
        var userDto = new Application.DTOs.UserProfileDto(
            user.Value.Id.Value,
            user.Value.Name.Value,
            user.Value.Email.Value,
            user.Value.Bio.Value,
            user.Value.ProfilePicture?.Path ?? string.Empty,
            user.Value.IsAdmin,
            user.Value.Skills.Select(s => new SkillDto(s.SkillId, s.Skill.Title.Value)).ToList());
        return Ok(userDto);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("tasty-cookies", new CookieOptions {
            Secure = false,
            SameSite = SameSiteMode.Lax,
            HttpOnly = false,
            Path = "/"
        });
        return Ok();
    }
} 