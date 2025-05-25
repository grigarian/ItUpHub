using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GrowSphere.Extentions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirst("userId")?.Value;
        return Guid.TryParse(id, out var result) ? result : (Guid?)null;
    }
}