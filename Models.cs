namespace UltraCommerce.Demo;

public sealed record PlatformBootstrap(
    string AppName,
    string Summary,
    string DefaultVendorId,
    IReadOnlyList<VendorOverview> Vendors);

public sealed record VendorOverview(
    string Id,
    string Name,
    string Slug,
    string SellerName,
    string Domain,
    string Tagline,
    string HeroTitle,
    string HeroCopy,
    string UniqueCode,
    string AccentTone);

public sealed record StorefrontQuery(
    string? VendorId,
    string? Search,
    string? CategoryId,
    string? Sort,
    bool? InStockOnly);

public sealed record StorefrontSnapshot(
    VendorOverview Vendor,
    IReadOnlyList<CategorySummary> Categories,
    IReadOnlyList<CampaignView> Campaigns,
    IReadOnlyList<ProductView> Products,
    StorefrontMetrics Metrics,
    ActiveFilters ActiveFilters);

public sealed record ActiveFilters(
    string Search,
    string CategoryId,
    string Sort,
    bool InStockOnly);

public sealed record CategorySummary(
    string Id,
    string Name,
    int SortOrder,
    int ProductCount);

public sealed record CampaignView(
    string Id,
    string VendorId,
    string Title,
    string Subtitle,
    string CallToAction,
    int Position,
    string AccentTone);

public sealed record ProductView(
    string Id,
    string VendorId,
    string CategoryId,
    string CategoryName,
    string Name,
    string Description,
    decimal Price,
    decimal? OriginalPrice,
    int Stock,
    double Rating,
    int Popularity,
    bool IsFeatured,
    string ImageUrl,
    IReadOnlyList<string> Tags);

public sealed record StorefrontMetrics(
    int TotalProducts,
    int InStockProducts,
    decimal AveragePrice,
    string TopCategory);

public sealed record AdminDashboard(
    VendorOverview Vendor,
    IReadOnlyList<CategorySummary> Categories,
    IReadOnlyList<CampaignView> Campaigns,
    IReadOnlyList<ProductView> Products,
    AdminMetrics Metrics);

public sealed record AdminMetrics(
    int SkuCount,
    int LowStockCount,
    int FeaturedCount,
    int CampaignCount);

public sealed record UpsertProductRequest(
    string VendorId,
    string? ProductId,
    string CategoryId,
    string Name,
    string Description,
    decimal Price,
    decimal? OriginalPrice,
    int Stock,
    bool IsFeatured,
    string Tags);

public sealed record UpsertCampaignRequest(
    string VendorId,
    string? CampaignId,
    string Title,
    string Subtitle,
    string CallToAction,
    int Position,
    string AccentTone);

public sealed record CartQuoteRequest(
    string VendorId,
    IReadOnlyList<CartLineRequest> Items);

public sealed record CartLineRequest(
    string ProductId,
    int Quantity);

public sealed record CartQuote(
    string VendorId,
    string Currency,
    IReadOnlyList<CartLineQuote> Items,
    decimal Subtotal,
    decimal Delivery,
    decimal Tax,
    decimal Total);

public sealed record CartLineQuote(
    string ProductId,
    string Name,
    int Quantity,
    int AvailableStock,
    decimal UnitPrice,
    decimal LineTotal);

public sealed record ApiError(
    string Code,
    string Message);

public sealed record OperationResult<T>(
    bool IsSuccess,
    T? Value,
    ApiError? Error)
{
    public static OperationResult<T> Success(T value) => new(true, value, null);

    public static OperationResult<T> Failure(string code, string message) =>
        new(false, default, new ApiError(code, message));
}

public static class ApiErrors
{
    public static ApiError VendorNotFound(string vendorId) =>
        new("vendor_not_found", $"Vendor '{vendorId}' was not found.");
}

internal sealed class Vendor
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Slug { get; init; }
    public required string SellerName { get; init; }
    public required string Domain { get; init; }
    public required string Tagline { get; init; }
    public required string HeroTitle { get; init; }
    public required string HeroCopy { get; init; }
    public required string UniqueCode { get; init; }
    public required string AccentTone { get; init; }
}

internal sealed class Category
{
    public required string Id { get; init; }
    public required string VendorId { get; init; }
    public required string Name { get; init; }
    public required int SortOrder { get; init; }
}

internal sealed class Campaign
{
    public required string Id { get; init; }
    public required string VendorId { get; init; }
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public required string CallToAction { get; set; }
    public required int Position { get; set; }
    public required string AccentTone { get; set; }
}

internal sealed class Product
{
    public required string Id { get; init; }
    public required string VendorId { get; init; }
    public required string CategoryId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public required int Stock { get; set; }
    public required double Rating { get; set; }
    public required int Popularity { get; set; }
    public required bool IsFeatured { get; set; }
    public required string ImageUrl { get; set; }
    public required List<string> Tags { get; set; }
}
