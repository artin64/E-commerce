using Microsoft.AspNetCore.Mvc;
using UltraCommerce.Demo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.WriteIndented = true;
});
builder.Services.AddSingleton<DemoDataStore>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/bootstrap", (DemoDataStore store) => Results.Ok(store.GetBootstrap()));

app.MapGet("/api/storefront", ([AsParameters] StorefrontQuery query, DemoDataStore store) =>
{
    var vendorId = string.IsNullOrWhiteSpace(query.VendorId) ? store.GetBootstrap().DefaultVendorId : query.VendorId!;
    return store.TryGetStorefront(vendorId, query, out var storefront)
        ? Results.Ok(storefront)
        : Results.NotFound(ApiErrors.VendorNotFound(vendorId));
});

app.MapGet("/api/admin/dashboard", ([FromQuery] string vendorId, DemoDataStore store) =>
    store.TryGetAdminDashboard(vendorId, out var dashboard)
        ? Results.Ok(dashboard)
        : Results.NotFound(ApiErrors.VendorNotFound(vendorId)));

app.MapPost("/api/admin/products", ([FromBody] UpsertProductRequest request, DemoDataStore store) =>
{
    var result = store.UpsertProduct(request);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
});

app.MapPut("/api/admin/products/{productId}", (string productId, [FromBody] UpsertProductRequest request, DemoDataStore store) =>
{
    var normalized = request with { ProductId = productId };
    var result = store.UpsertProduct(normalized);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
});

app.MapDelete("/api/admin/products/{productId}", (string productId, [FromQuery] string vendorId, DemoDataStore store) =>
{
    var result = store.DeleteProduct(vendorId, productId);
    return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
});

app.MapPost("/api/admin/campaigns", ([FromBody] UpsertCampaignRequest request, DemoDataStore store) =>
{
    var result = store.UpsertCampaign(request);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
});

app.MapPut("/api/admin/campaigns/{campaignId}", (string campaignId, [FromBody] UpsertCampaignRequest request, DemoDataStore store) =>
{
    var normalized = request with { CampaignId = campaignId };
    var result = store.UpsertCampaign(normalized);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
});

app.MapDelete("/api/admin/campaigns/{campaignId}", (string campaignId, [FromQuery] string vendorId, DemoDataStore store) =>
{
    var result = store.DeleteCampaign(vendorId, campaignId);
    return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
});

app.MapPost("/api/cart/quote", ([FromBody] CartQuoteRequest request, DemoDataStore store) =>
{
    var result = store.CalculateCartQuote(request);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
});

app.MapFallbackToFile("index.html");

app.Run();
