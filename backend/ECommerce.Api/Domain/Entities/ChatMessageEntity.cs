namespace ECommerce.Api.Domain.Entities;

public sealed class ChatMessageEntity
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }
    public StoreEntity? Store { get; set; }

    public Guid? ProductId { get; set; }
    public ProductEntity? Product { get; set; }

    public Guid BuyerUserId { get; set; }
    public UserEntity? BuyerUser { get; set; }

    public Guid VendorUserId { get; set; }
    public UserEntity? VendorUser { get; set; }

    public string Sender { get; set; } = "Buyer"; // Buyer/Vendor/AI
    public string Body { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

