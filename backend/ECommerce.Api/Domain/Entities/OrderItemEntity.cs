namespace ECommerce.Api.Domain.Entities;

public sealed class OrderItemEntity
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public OrderEntity? Order { get; set; }

    public Guid ProductId { get; set; }
    public ProductEntity? Product { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

