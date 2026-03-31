namespace ECommerce.Api.Domain.Dtos;

public sealed record RegisterRequest(string Name, string Email, string Password, string Role, string? StoreName);
public sealed record LoginRequest(string Email, string Password);

public sealed record AuthResponse(
    string Token,
    string UserId,
    string Name,
    string Email,
    string Role,
    string? StoreId,
    string? StoreKey
);

