namespace ECommerce.Api.Domain.Entities;

public sealed class StoreEntity
{
    public Guid Id { get; set; }

    // Public store slug/id shown to buyers.
    public string StoreKey { get; set; } = string.Empty; // e.g. "store-asos-tech"

    public string Name { get; set; } = string.Empty;

    public Guid OwnerUserId { get; set; }
    public UserEntity? OwnerUser { get; set; }

    public bool IsVerified { get; set; }
    public bool IsActive { get; set; } = true;

    // Theme / customization JSON (simple for now).
    public string ThemeJson { get; set; } = "{}";

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public List<CategoryEntity> Categories { get; set; } = [];
    public List<ProductEntity> Products { get; set; } = [];
    public List<AdEntity> Ads { get; set; } = [];
    public List<GiftCardEntity> GiftCards { get; set; } = [];
}

