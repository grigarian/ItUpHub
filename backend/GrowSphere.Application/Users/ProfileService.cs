using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Interfaces;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GrowSphere.Application.Users;

public class ProfileService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProfileService> _logger;
    private readonly IFileStorage _fileStorage;
    private readonly ICurrentUserService _currentUserService;
    
    public ProfileService(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<ProfileService> logger,
        IFileStorage fileStorage)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _fileStorage = fileStorage;
    }

    public async Task<Result<Bio, Error>> UpdateUserBio(
        UserProfileRequest userProfileRequest,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var user = await _userRepository.GetById(userId.Value, cancellationToken);
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
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var userResult = await _userRepository.GetById(userId.Value, cancellationToken);
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
} 