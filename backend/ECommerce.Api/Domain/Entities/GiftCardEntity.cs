namespace ECommerce.Api.Domain.Entities;

public sealed class GiftCardEntity
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }
    public StoreEntity? Store { get; set; }

    public string Code { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? ExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

