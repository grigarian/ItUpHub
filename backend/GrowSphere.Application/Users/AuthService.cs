using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using GrowSphere.Application.DTOs;
using GrowSphere.Application.Interfaces;
using GrowSphere.Application.Interfaces.Auth;
using GrowSphere.Core;
using GrowSphere.Domain;
using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.UserModel;
using GrowSphere.Infrastructure;
using Microsoft.Extensions.Logging;

namespace GrowSphere.Application.Users;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IJwtProvider jwtProvider,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Register(
        RegisterUserRequest registerUserRequest,
        CancellationToken cancellationToken)
    {
        var checkedEmail = await _userRepository.GetByEmail(registerUserRequest.Email);
        if (!checkedEmail.IsFailure)
        {
            if (checkedEmail.Value.Email.Value == registerUserRequest.Email)
                return Errors.UserError.InvalidEmail(registerUserRequest.Email);
        }

        if (!PasswordIsValid(registerUserRequest.Password))
        {
            return Errors.UserError.InvalidPassword();
        }

        var hashedPassword = _passwordHasher.Generate(registerUserRequest.Password);
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
        var createDate = DateTime.UtcNow;
        var user = User.Register(userId,
            userName.Value,
            email.Value,
            hashedPassword,
            createDate,
            Bio.Create(string.Empty).Value,
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
        if (user.IsFailure)
            return user.Error;
        var result = _passwordHasher.Verify(loginUserRequest.Password, user.Value.PasswordHash);
        if (!result)
            return Errors.UserError.FailedLogin(loginUserRequest.Email);
        var token = _jwtProvider.GenerateToken(user.Value);
        return token;
    }

    private bool PasswordIsValid(string password)
    {
        int minLenght = 8;
        bool hasUpper = password.Any(char.IsUpper);
        bool hasLower = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecial = Regex.IsMatch(password, @"[\W_]");
        if (hasUpper && hasLower && hasDigit && hasSpecial && password.Length >= minLenght)
            return true;
        return false;
    }
}
