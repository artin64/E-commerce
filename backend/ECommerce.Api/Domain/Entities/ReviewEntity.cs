namespace ECommerce.Api.Domain.Entities;

public sealed class ReviewEntity
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }
    public StoreEntity? Store { get; set; }

    public Guid ProductId { get; set; }
    public ProductEntity? Product { get; set; }

    public Guid BuyerUserId { get; set; }
    public UserEntity? BuyerUser { get; set; }

    public int Rating { get; set; } // 1..5
    public string Title { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

