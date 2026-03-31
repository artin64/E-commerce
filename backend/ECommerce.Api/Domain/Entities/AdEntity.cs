namespace ECommerce.Api.Domain.Entities;

public sealed class AdEntity
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }
    public StoreEntity? Store { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Placement { get; set; } = "Top"; // Top / MidGrid / SideRail
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public string ImageUrl { get; set; } = string.Empty;
    public string LinkUrl { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

