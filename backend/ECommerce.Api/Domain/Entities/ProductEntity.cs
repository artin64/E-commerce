namespace ECommerce.Api.Domain.Entities;

public sealed class ProductEntity
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }
    public StoreEntity? Store { get; set; }

    public Guid? CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; } = true;

    public string TagsCsv { get; set; } = string.Empty;
    public string ImagesJson { get; set; } = "[]";

    public int Popularity { get; set; }
    public double RatingAvg { get; set; }
    public int RatingCount { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}

