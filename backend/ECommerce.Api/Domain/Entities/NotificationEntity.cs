namespace ECommerce.Api.Domain.Entities;

public sealed class NotificationEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    public Guid? StoreId { get; set; }
    public StoreEntity? Store { get; set; }

    public string Type { get; set; } = "Order"; // Order/Offer/Product/Message
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

