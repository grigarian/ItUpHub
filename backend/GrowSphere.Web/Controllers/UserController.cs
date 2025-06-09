using System.Security.Claims;
using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Application.Skills;
using GrowSphere.Application.Users;
using GrowSphere.Domain;
using GrowSphere.Domain.Interfaces;
using GrowSphere.Extentions;
using GrowSphere.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GrowSphere.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly CurrentUserService _currentUserService;
    private readonly IFileStorage _fileStorage;

    public UserController(
        UserRepository userRepository,
        CurrentUserService currentUserService,
        IFileStorage fileStorage)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _fileStorage = fileStorage;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id,
        [FromServices] SkillService skillService,
        CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetById(id, cancellationToken);

        if (userResult.IsFailure)
            return userResult.Error.ToResponse();
        
        var user = userResult.Value;
        
        var skills = await skillService.GetByUserId(user.Id, cancellationToken);
        
        if(skills.IsFailure)
            return skills.Error.ToResponse();

        // Формируем DTO с нужными данными пользователя
        var userDto = new UserProfileDto(
            user.Id.Value,
            user.Name.Value,
            user.Email.Value,
            user.Bio.Value,
            user.ProfilePicture.Path,
            user.IsAdmin,
            skills.Value);
        
        return Ok(userDto);
    }

    [HttpPost("register")]
    public async Task<ActionResult<Guid>> Register(
        [FromServices] UserService service,
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.Register(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        Log.Information
        ("User {UserName} successfully registered on {Time}",
            request.UserName,
            DateTime.Now);
        
        return result.Value;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromServices] UserService service,
        [FromServices] SkillService skillService,
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var token = await service.Login(request, cancellationToken);
        
        if(token.IsFailure)
            return token.Error.ToResponse();

        var user = await _userRepository.GetByEmail(request.Email);
        
        if(user.IsFailure)
            return user.Error.ToResponse();

        var skills = await skillService.GetByUserId(user.Value.Id, cancellationToken);
        
        if(skills.IsFailure)
            return skills.Error.ToResponse();

        var context = HttpContext;
        
        context.Response.Cookies.Append("tasty-cookies", token.Value, new CookieOptions {
            Secure = false,  // Разрешаем HTTP
            SameSite = SameSiteMode.None,  // Разрешаем кросс-сайтовые куки
            HttpOnly = false,  // Разрешаем доступ из JavaScript
            Path = "/",
            Expires = DateTime.UtcNow.AddDays(30)  // Кука будет жить 30 дней
            // Убираем Domain, чтобы кука работала для всех доменов
        });
        
        Log.Information
        ("User {UserName} successfully login on {Time}",
            request.Email,
            DateTime.Now);
        
        var userDto = new UserProfileDto(
            user.Value.Id.Value,
            user.Value.Name.Value,
            user.Value.Email.Value,
            user.Value.Bio.Value,
            user.Value.ProfilePicture?.Path ?? string.Empty,
            user.Value.IsAdmin,
            skills.Value);

        return Ok(userDto);
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("tasty-cookies", new CookieOptions {
            Secure = false,
            SameSite = SameSiteMode.None,
            HttpOnly = false,
            Path = "/"
        });
    
        return Ok();
    }

    [Authorize]
    [HttpPut("{userId}/bio")]
    public async Task<IActionResult> UpdateBio(
        Guid userId,
        [FromServices] UserService service,
        [FromBody] UserProfileRequest request,
        CancellationToken cancellationToken)
    {
        

        if (userId == Guid.Empty)
            return Errors.General.Unauthorized().ToResponse();
        
        var result = await service.UpdateUserBio(userId, request, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        Log.Information
            ("User {UserId} updated bio on {Time}",
                userId,
                DateTime.Now);
        
        return Ok($"{result.Value}");
    }
    
    [Authorize]
    [HttpPost("{userId}/profile-image")]
    public async Task<IActionResult> UploadProfileImage(
        Guid userId,
        IFormFile file,
        [FromServices] UserService service,
        CancellationToken cancellationToken)
    {
        var result = await service.UploadImage(userId, file, cancellationToken);
        return result.Match(
            picture => Ok(new { picturePath = picture.Path }),
            error => Problem(error.Message)
        );
    }
    
    [Authorize]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUser(
        [FromServices] SkillService skillService,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId == null || userId == Guid.Empty)
            return Unauthorized(new { message = "Пользователь не авторизован" });

        var userResult = await _userRepository.GetById(userId, cancellationToken);

        if (userResult.IsFailure)
            return userResult.Error.ToResponse();

        var user = userResult.Value;
        
        var skills = await skillService.GetByUserId(user.Id, cancellationToken);
        
        if(skills.IsFailure)
            return skills.Error.ToResponse();

        // Формируем DTO с нужными данными пользователя
        var userDto = new UserProfileDto(
            user.Id.Value,
            user.Name.Value,
            user.Email.Value,
            user.Bio.Value,
            user.ProfilePicture?.Path ?? string.Empty,
            user.IsAdmin,
            skills.Value);

        return Ok(userDto);
    }
    
    [Authorize]
    [HttpPost("{userId}/skill")]
    public async Task<IActionResult> AddSkill
    (
        Guid userId,
        [FromBody] AddSkillToUserRequest request,
        [FromServices] UserService service,
        CancellationToken cancellationToken
    )
    {
        var result = await service.AddSkill(userId, request, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpDelete("{userId:guid}/skills/{skillId:guid}")]
    public async Task<IActionResult> RemoveSkill
        (
            Guid userId,
            Guid skillId,
            [FromServices] UserService userService,
            [FromServices] ISkillRepository skillRepository,
            CancellationToken cancellationToken
        )
    {
        if (_currentUserService.UserId != userId)
            return Forbid();

        var result = await userService.RemoveSkill(userId, skillId, skillRepository, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpPost("{userId}/project")]
    public async Task<IActionResult> AddProject
    (
        Guid userId,
        Guid projectId,
        [FromServices] UserService service,
        CancellationToken cancellationToken
    )
    {
        var result = await service.AddProject(userId, projectId, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpDelete("{userId:guid}/projects/{projectId:guid}")]
    public async Task<IActionResult> RemoveProject
    (
        Guid userId,
        Guid projectId,
        [FromServices] UserService userService,
        CancellationToken cancellationToken
    )
    {
        if (_currentUserService.UserId != userId)
            return Forbid();

        var result = await userService.RemoveProject(userId, projectId, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet("images/{*path}")]
    public async Task<IActionResult> GetImage(string path)
    {
        try
        {
            var decodedPath = Uri.UnescapeDataString(path);
            var stream = await _fileStorage.GetFileAsync(decodedPath);
            return File(stream, "image/jpeg");
        }
        catch
        {
            return NotFound();
        }
    }
    
    [Authorize]
    [HttpPost("{userId}/set-admin")]
    public async Task<IActionResult> SetUserAsAdmin(
        Guid userId,
        [FromServices] UserService service,
        CancellationToken cancellationToken)
    {
        var currentUser = await _userRepository.GetById(_currentUserService.UserId, cancellationToken);
        
        //if(currentUser.IsFailure)
        //      turn currentUser.Error.ToResponse();
            
        //if(!currentUser.Value.IsAdmin)
           // return Errors.General.NotFound().ToResponse();
            
        var result = await service.SetUserAsAdmin(userId, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers(
        [FromServices] CurrentUserService service,
        CancellationToken cancellationToken)
    {
        //var currentUser = await _userRepository.GetById(service.UserId, cancellationToken);
        
        //if(currentUser.IsFailure)
        //    return currentUser.Error.ToResponse();
            
        // if(!currentUser.Value.IsAdmin)
        //     return Forbid();
            
        var users = await _userRepository.GetAll(cancellationToken);
        
        if(users.IsFailure)
            return users.Error.ToResponse();
            
        var userDtos = users.Value.Select(u => new Application.DTOs.UserProfileDto(
            u.Id.Value,
            u.Name.Value,
            u.Email.Value,
            u.Bio.Value,
            u.ProfilePicture?.Path ?? string.Empty,
            u.IsAdmin,
            u.Skills.Select(s => s.Skill).ToList()
        ));
        
        return Ok(userDtos);
    }
}