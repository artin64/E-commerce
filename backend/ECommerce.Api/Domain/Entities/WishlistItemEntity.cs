namespace ECommerce.Api.Domain.Entities;

public sealed class WishlistItemEntity
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }
    public StoreEntity? Store { get; set; }

    public Guid BuyerUserId { get; set; }
    public UserEntity? BuyerUser { get; set; }

    public Guid ProductId { get; set; }
    public ProductEntity? Product { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

