namespace ECommerce.Api.Domain.Entities;

public sealed class CategoryEntity
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }
    public StoreEntity? Store { get; set; }

    public string Name { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

