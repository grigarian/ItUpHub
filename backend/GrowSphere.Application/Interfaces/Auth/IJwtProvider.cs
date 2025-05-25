using GrowSphere.Domain.Models.UserModel;

namespace GrowSphere.Application.Interfaces.Auth;

public interface IJwtProvider
{
    string GenerateToken(User user);
}