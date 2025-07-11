using CSharpFunctionalExtensions;
using GrowSphere.Application.Interfaces;
using GrowSphere.Core;
using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Application.Users;

public class AdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdminService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<User, Error>> SetUserAsAdmin(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(userId, cancellationToken);
        if(user.IsFailure)
            return user.Error;
        user.Value.SetAsAdmin();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Value;
    }
} 