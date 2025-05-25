using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Application.Interfaces.Auth;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Interfaces;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.UserModel;
using GrowSphere.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrowSphere.Application.Users;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly ILogger<UserService> _logger;
    private readonly IFileStorage _fileStorage;
    private readonly IProjectRepository _projectRepository;
    
    public UserService(
        IJwtProvider jwtProvider,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        ILogger<UserService> logger,
        IFileStorage fileStorage,
        ISkillRepository skillRepository,
        IProjectRepository projectRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _fileStorage = fileStorage;
        _skillRepository = skillRepository;
        _projectRepository = projectRepository;
    }
    
    public async Task<Result<Guid,Error>> Register(
        RegisterUserRequest registerUserRequest,
        CancellationToken cancellationToken)
    {
        var checkedEmail = _userRepository.GetByEmail(registerUserRequest.Email).Result;
        
        if (!checkedEmail.IsFailure)
        {
            if(checkedEmail.Value.Email.Value == registerUserRequest.Email)
                    return Errors.UserError.InvalidEmail(registerUserRequest.Email);
        }

        if (!PasswordIsValid(registerUserRequest.Password))
        {
            return Errors.UserError.InvalidPassword();
        }
        
        var hashedPassword= _passwordHasher.Generate(registerUserRequest.Password);
        
        var userId = UserId.NewId();
        
        var userName = UserName.Create(registerUserRequest.UserName);

        if (userName.IsFailure)
        {
            return userName.Error;
        }
        
        var email = Email.Create(registerUserRequest.Email);

        if (email.IsFailure)
        {
            return email.Error;
        }

        var createDate = registerUserRequest.CreatedDate;
        
        var user = User.Register(userId,
            userName.Value,
            email.Value,
            hashedPassword,
            createDate,
            Bio.Create(String.Empty).Value,
            Picture.Empty()
            );
        
        if (user.IsFailure)
        {
            return user.Error;
        }
        
        await _userRepository.Add(user.Value, cancellationToken);
        
        return userId.Value;
    }

    public async Task<Result<string, Error>> Login(
        LoginUserRequest loginUserRequest,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(loginUserRequest.Email);
        
        if(user.IsFailure)
            return user.Error;

        var result = _passwordHasher.Verify(loginUserRequest.Password, user.Value.PasswordHash);

        if (result == false)
            return Errors.UserError.FailedLogin(loginUserRequest.Email);
        
        var token = _jwtProvider.GenerateToken(user.Value);
        
        return token;
    }

    public async Task<Result<Bio, Error>> UpdateUserBio(
        Guid userId,
        UserProfileRequest userProfileRequest,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(userId, cancellationToken);
        
        if(user.IsFailure)
            return user.Error;
        
        var bio = Bio.Create(userProfileRequest.Bio);
        
        if (bio.IsFailure)
            return bio.Error;
        
        user.Value.UpdateBio(bio.Value);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return bio.Value;
    }
    
    public async Task<Result<Picture, Error>> UploadImage(
        Guid userId,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetById(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.Error;

        // Валидация файла
        if (file == null || file.Length == 0)
            return Errors.FileError.EmptyFile();

        if (!file.ContentType.StartsWith("image/"))
            return Errors.FileError.InvalidType("image/jpeg, image/png");

        // Генерация пути
        var filePath = $"user/{userId}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        using var stream = file.OpenReadStream();
        var uploadResult = await _fileStorage.UploadFileAsync(stream, filePath, file.ContentType);
        
        var pictureResult = Picture.Create(uploadResult.Path, file.ContentType);
        if (pictureResult.IsFailure)
            return pictureResult.Error;

        userResult.Value.UpdateProfilePicture(pictureResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return pictureResult.Value;
    }

    public async Task<Result<Skill, Error>> AddSkill
        (
            Guid userId,
            AddSkillToUserRequest request,
            CancellationToken cancellationToken
        )
    {
        var user = await _userRepository.GetById(userId, cancellationToken);
        if (user.IsFailure)
            return user.Error;
        
        var skill = await _skillRepository.GetById(request.SkillId, cancellationToken);
        if(skill.IsFailure)
            return skill.Error;
        
        var userSkills = await _skillRepository
            .GetSkillsByUserIdAsync(userId, cancellationToken);

        if (userSkills.Value.Any(s => s.Id == request.SkillId))
        {
            return Errors.General.ValueIsInvalid("Skill Id already exists");
        }
        
        user.Value.AddSkill(skill.Value);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return skill.Value;
    }

    public async Task<Result<Guid, Error>> RemoveSkill
    (
        Guid userId,
        Guid skillId,
        [FromServices] ISkillRepository skillRepository,
        CancellationToken cancellationToken
    )
    {
        var userIdObj = UserId.Create(userId);
        var skillIdObj = SkillId.Create(skillId);

        if (userIdObj.Value == Guid.Empty || skillIdObj.Value == Guid.Empty)
            return Errors.General.NotFound();
        
        return await _userRepository.RemoveSkill(userIdObj, skillIdObj, skillRepository, cancellationToken);
    }
    
    public async Task<Result<Project, Error>> AddProject
    (
        Guid userId,
        Guid projectId,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetById(userId, cancellationToken);
        if (user.IsFailure)
            return user.Error;
        
        var project = await _projectRepository.GetById(projectId, cancellationToken);
        if(project.IsFailure)
            return project.Error;
        
        var userProjects = await _projectRepository
            .GetAllByUserId(userId, cancellationToken);

        if (userProjects.Value.Any(s => s.Id == projectId))
        {
            return Errors.General.ValueIsInvalid("Project Id already exists");
        }
        
        user.Value.AddProject(project.Value);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return project.Value;
    }
    
    public async Task<Result<Guid, Error>> RemoveProject
    (
        Guid userId,
        Guid projectId,
        CancellationToken cancellationToken
    )
    {
        var userIdObj = UserId.Create(userId);
        var projectIdObj = ProjectId.Create(projectId);

        if (userIdObj.Value == Guid.Empty || projectIdObj.Value == Guid.Empty)
            return Errors.General.NotFound();
        
        return await _userRepository.RemoveProject(userIdObj, projectIdObj, cancellationToken);
    }

    private bool PasswordIsValid(string password)
    {
        int minLenght = 8;
        bool hasUpper = password.Any(char.IsUpper);
        bool hasLower = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecial = Regex.IsMatch(password, @"[\W_]");
        if(hasUpper && hasLower && hasDigit && hasSpecial && password.Length >= minLenght)
            return true;
        return false;
    }
    
    
}