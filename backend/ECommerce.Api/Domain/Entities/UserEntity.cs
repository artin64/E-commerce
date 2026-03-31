using ECommerce.Api.Domain.Enums;

namespace ECommerce.Api.Domain.Entities;

public sealed class UserEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Store hashed password only (BCrypt).
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Buyer;

    // Vendor user can own a store (optional).
    public Guid? StoreId { get; set; }
    public StoreEntity? Store { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

