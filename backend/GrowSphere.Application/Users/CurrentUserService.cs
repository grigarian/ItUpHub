using GrowSphere.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GrowSphere.Application.Users;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.
                HttpContext?.
                User?.
                FindFirst("userId")?.Value;
            
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}