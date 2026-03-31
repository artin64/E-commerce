using System.Text.RegularExpressions;
using BCrypt.Net;
using ECommerce.Api.Data;
using ECommerce.Api.Domain.Dtos;
using ECommerce.Api.Domain.Entities;
using ECommerce.Api.Domain.Enums;
using ECommerce.Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwt;

    public AuthController(AppDbContext db, JwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req, CancellationToken ct)
    {
        var email = (req.Email ?? string.Empty).Trim().ToLowerInvariant();
        var name = (req.Name ?? string.Empty).Trim();
        var password = req.Password ?? string.Empty;
        var roleStr = (req.Role ?? "Buyer").Trim();

        if (name.Length < 2) return BadRequest("Name too short.");
        if (!IsEmail(email)) return BadRequest("Invalid email.");
        if (password.Length < 6) return BadRequest("Password must be at least 6 chars.");

        var exists = await _db.Users.AnyAsync(u => u.Email == email, ct);
        if (exists) return Conflict("Email already registered.");

        var role = ParseRole(roleStr);
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role,
            CreatedAtUtc = DateTime.UtcNow,
        };

        StoreEntity? store = null;
        if (role == UserRole.Vendor)
        {
            var storeName = (req.StoreName ?? string.Empty).Trim();
            if (storeName.Length < 2) return BadRequest("StoreName is required for Vendor.");

            store = new StoreEntity
            {
                Id = Guid.NewGuid(),
                StoreKey = Slugify(storeName) + "-" + ShortId(),
                Name = storeName,
                OwnerUserId = user.Id,
                IsActive = true,
                IsVerified = false,
                ThemeJson = "{}",
                CreatedAtUtc = DateTime.UtcNow,
            };

            user.StoreId = store.Id;
            _db.Stores.Add(store);
        }

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        var token = _jwt.CreateToken(user);
        return Ok(new AuthResponse(
            Token: token,
            UserId: user.Id.ToString(),
            Name: user.Name,
            Email: user.Email,
            Role: user.Role.ToString(),
            StoreId: user.StoreId?.ToString(),
            StoreKey: store?.StoreKey
        ));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req, CancellationToken ct)
    {
        var email = (req.Email ?? string.Empty).Trim().ToLowerInvariant();
        var password = req.Password ?? string.Empty;

        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null) return Unauthorized("Invalid credentials.");

        var ok = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!ok) return Unauthorized("Invalid credentials.");

        StoreEntity? store = null;
        if (user.StoreId.HasValue)
        {
            store = await _db.Stores.AsNoTracking().FirstOrDefaultAsync(s => s.Id == user.StoreId.Value, ct);
        }

        var token = _jwt.CreateToken(user);
        return Ok(new AuthResponse(
            Token: token,
            UserId: user.Id.ToString(),
            Name: user.Name,
            Email: user.Email,
            Role: user.Role.ToString(),
            StoreId: user.StoreId?.ToString(),
            StoreKey: store?.StoreKey
        ));
    }

    private static bool IsEmail(string email) =>
        email.Length <= 320 && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    private static UserRole ParseRole(string role)
    {
        return role.ToLowerInvariant() switch
        {
            "buyer" => UserRole.Buyer,
            "vendor" => UserRole.Vendor,
            "superadmin" => UserRole.SuperAdmin,
            _ => UserRole.Buyer
        };
    }

    private static string ShortId() => Guid.NewGuid().ToString("N")[..6];

    private static string Slugify(string s)
    {
        s = s.Trim().ToLowerInvariant();
        s = Regex.Replace(s, @"\s+", "-");
        s = Regex.Replace(s, @"[^a-z0-9\-]", "");
        s = Regex.Replace(s, @"-+", "-");
        return s.Length == 0 ? "store" : s;
    }
}

