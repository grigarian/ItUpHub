using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GrowSphere.Application.Interfaces.Auth;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GrowSphere.Infrastructure;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public string GenerateToken(User user)
    {
        Claim[] claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
            new("userId", user.Id.Value.ToString()) 
        ];
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials
        );
        
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}