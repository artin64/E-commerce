using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.Api.Domain.Entities;
using ECommerce.Api.Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Api.Services.Auth;

public sealed class JwtTokenService
{
    private readonly JwtOptions _opts;

    public JwtTokenService(IOptions<JwtOptions> opts)
    {
        _opts = opts.Value;
    }

    public string CreateToken(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("role", user.Role.ToString()),
        };

        if (user.Role == UserRole.Vendor && user.StoreId.HasValue)
            claims.Add(new("storeId", user.StoreId.Value.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _opts.Issuer,
            audience: _opts.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_opts.ExpMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

