using ECommerce.Api.Domain.Enums;

namespace ECommerce.Api.Domain.Entities;

public sealed class OrderEntity
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }
    public StoreEntity? Store { get; set; }

    public Guid BuyerUserId { get; set; }
    public UserEntity? BuyerUser { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }

    public string BuyerEmail { get; set; } = string.Empty;
    public string BuyerAddress { get; set; } = string.Empty;

    public string TrackingNumber { get; set; } = string.Empty;
    public string ShippingStatus { get; set; } = string.Empty;

    public string GiftCardCode { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public List<OrderItemEntity> Items { get; set; } = [];
}

