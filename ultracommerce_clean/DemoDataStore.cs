using System.Globalization;

namespace UltraCommerce.Demo;

public sealed class DemoDataStore
{
    private readonly object _syncRoot = new();
    private readonly List<Vendor> _vendors;
    private readonly List<Category> _categories;
    private readonly List<Campaign> _campaigns;
    private readonly List<Product> _products;

    public DemoDataStore()
    {
        _vendors =
        [
            new Vendor
            {
                Id = "urban-vault",
                Name = "Urban Vault",
                Slug = "urban-vault",
                SellerName = "Ardit Krasniqi",
                Domain = "urbanvault.market.demo",
                Tagline = "Streetwear essentials curated for fast-moving city teams.",
                HeroTitle = "Merchandising that feels editorial, not cluttered.",
                HeroCopy = "A demo-ready multi-vendor storefront where the seller updates catalog and campaigns live while the buyer filters, discovers and checks totals instantly.",
                UniqueCode = "UV-2026-001",
                AccentTone = "sunset"
            },
            new Vendor
            {
                Id = "northlane-active",
                Name = "Northlane Active",
                Slug = "northlane-active",
                SellerName = "Blendor Gashi",
                Domain = "northlane.market.demo",
                Tagline = "Performance apparel for commuting, training and travel.",
                HeroTitle = "One platform, isolated stores, faster seller execution.",
                HeroCopy = "Northlane shows the same core engine with a different catalog, messaging and campaigns to prove vendor-level separation inside one SaaS commerce platform.",
                UniqueCode = "NL-2026-014",
                AccentTone = "electric"
            }
        ];

        _categories =
        [
            new Category { Id = "uv-sneakers", VendorId = "urban-vault", Name = "Sneakers", SortOrder = 1 },
            new Category { Id = "uv-outerwear", VendorId = "urban-vault", Name = "Outerwear", SortOrder = 2 },
            new Category { Id = "uv-essentials", VendorId = "urban-vault", Name = "Essentials", SortOrder = 3 },
            new Category { Id = "uv-accessories", VendorId = "urban-vault", Name = "Accessories", SortOrder = 4 },
            new Category { Id = "nl-running", VendorId = "northlane-active", Name = "Running", SortOrder = 1 },
            new Category { Id = "nl-layering", VendorId = "northlane-active", Name = "Layering", SortOrder = 2 },
            new Category { Id = "nl-recovery", VendorId = "northlane-active", Name = "Recovery", SortOrder = 3 },
            new Category { Id = "nl-travel", VendorId = "northlane-active", Name = "Travel", SortOrder = 4 }
        ];

        _campaigns =
        [
            new Campaign
            {
                Id = "cmp-uv-city-reset",
                VendorId = "urban-vault",
                Title = "City Reset",
                Subtitle = "Overshirts, utility cargos and muted tones merchandised for the next weekly drop.",
                CallToAction = "Explore the edit",
                Position = 1,
                AccentTone = "sunset"
            },
            new Campaign
            {
                Id = "cmp-uv-weekend-layer",
                VendorId = "urban-vault",
                Title = "Weekend Layering",
                Subtitle = "A second card reserved for high-margin looks and promo storytelling.",
                CallToAction = "View hero pieces",
                Position = 2,
                AccentTone = "graphite"
            },
            new Campaign
            {
                Id = "cmp-nl-race-week",
                VendorId = "northlane-active",
                Title = "Race Week Stack",
                Subtitle = "Breathable shells, training tops and travel kits grouped into one performance narrative.",
                CallToAction = "Build the set",
                Position = 1,
                AccentTone = "electric"
            },
            new Campaign
            {
                Id = "cmp-nl-recovery",
                VendorId = "northlane-active",
                Title = "Recovery Mode",
                Subtitle = "Foam slides and lounge layers promoted as post-run best sellers.",
                CallToAction = "See recovery picks",
                Position = 2,
                AccentTone = "forest"
            }
        ];

        _products =
        [
            SeedProduct("uv-p1", "urban-vault", "uv-sneakers", "Metro Runner XT", "Mesh runner with editorial contrast panels and fast city styling.", 119.90m, 149.90m, 16, 4.7, 93, true, ["new-in", "street", "lightweight"]),
            SeedProduct("uv-p2", "urban-vault", "uv-outerwear", "Signal Overshirt", "Midweight overshirt designed for transit, office and after-hours layering.", 89.00m, null, 11, 4.6, 88, true, ["editor-pick", "layering"]),
            SeedProduct("uv-p3", "urban-vault", "uv-essentials", "Canvas Utility Pant", "Tapered cargo with stretch panels and a clean modern fit.", 76.50m, 95.00m, 24, 4.4, 81, false, ["best-seller", "cargo"]),
            SeedProduct("uv-p4", "urban-vault", "uv-essentials", "Studio Tee Pack", "Two premium heavyweight tees for the baseline wardrobe refresh.", 42.00m, null, 32, 4.3, 74, false, ["bundle", "core"]),
            SeedProduct("uv-p5", "urban-vault", "uv-accessories", "Transit Crossbody", "Minimal crossbody built for phone, wallet and quick-carry essentials.", 54.99m, null, 9, 4.8, 77, true, ["accessory", "travel"]),
            SeedProduct("uv-p6", "urban-vault", "uv-outerwear", "Noir Coach Jacket", "Light coach jacket with concealed snap placket and matte finish.", 132.00m, 165.00m, 7, 4.5, 85, true, ["limited", "outerwear"]),
            SeedProduct("nl-p1", "northlane-active", "nl-running", "AeroPulse Shell", "Weather-ready shell for early runs and aggressive wind protection.", 148.00m, 179.00m, 8, 4.8, 96, true, ["performance", "weather"]),
            SeedProduct("nl-p2", "northlane-active", "nl-running", "StrideKnit Tee", "Sweat-wicking technical top built for intervals and long tempo blocks.", 49.00m, null, 27, 4.5, 83, false, ["breathable", "training"]),
            SeedProduct("nl-p3", "northlane-active", "nl-layering", "Transit Warm Midlayer", "Structured half-zip that moves from training to airport lounge cleanly.", 94.00m, null, 13, 4.7, 86, true, ["versatile", "travel"]),
            SeedProduct("nl-p4", "northlane-active", "nl-recovery", "Cloud Reset Slides", "Contoured recovery slides for post-run comfort and everyday wear.", 39.00m, null, 19, 4.4, 71, false, ["recovery", "comfort"]),
            SeedProduct("nl-p5", "northlane-active", "nl-travel", "Carry Grid Duffel", "Segmented duffel designed for race kit, shoes and electronics.", 118.00m, 142.00m, 6, 4.6, 79, true, ["duffel", "organized"]),
            SeedProduct("nl-p6", "northlane-active", "nl-layering", "Pace Jogger Pro", "Technical jogger with zip pockets and adaptive stretch construction.", 82.50m, null, 17, 4.5, 80, false, ["training", "commute"])
        ];
    }

    public PlatformBootstrap GetBootstrap() =>
        new(
            "NexaMarket Commerce Cloud",
            "A multi-vendor commerce demo focused on seller control, buyer discovery and presentation-ready clarity.",
            _vendors.First().Id,
            _vendors.Select(MapVendorOverview).ToList());

    public bool TryGetStorefront(string vendorId, StorefrontQuery query, out StorefrontSnapshot snapshot)
    {
        lock (_syncRoot)
        {
            var vendor = _vendors.FirstOrDefault(v => v.Id == vendorId);
            if (vendor is null)
            {
                snapshot = default!;
                return false;
            }

            var categories = _categories
                .Where(category => category.VendorId == vendorId)
                .OrderBy(category => category.SortOrder)
                .ToList();

            IEnumerable<Product> filteredProducts = _products.Where(product => product.VendorId == vendorId);

            var search = (query.Search ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredProducts = filteredProducts.Where(product =>
                    product.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                    || product.Description.Contains(search, StringComparison.OrdinalIgnoreCase)
                    || product.Tags.Any(tag => tag.Contains(search, StringComparison.OrdinalIgnoreCase)));
            }

            var categoryId = string.IsNullOrWhiteSpace(query.CategoryId) ? "all" : query.CategoryId!;
            if (!string.Equals(categoryId, "all", StringComparison.OrdinalIgnoreCase))
            {
                filteredProducts = filteredProducts.Where(product => product.CategoryId == categoryId);
            }

            var inStockOnly = query.InStockOnly ?? false;
            if (inStockOnly)
            {
                filteredProducts = filteredProducts.Where(product => product.Stock > 0);
            }

            var sort = string.IsNullOrWhiteSpace(query.Sort) ? "featured" : query.Sort!.ToLowerInvariant();
            filteredProducts = sort switch
            {
                "price-asc" => filteredProducts.OrderBy(product => product.Price).ThenByDescending(product => product.Popularity),
                "price-desc" => filteredProducts.OrderByDescending(product => product.Price).ThenByDescending(product => product.Popularity),
                "rating" => filteredProducts.OrderByDescending(product => product.Rating).ThenByDescending(product => product.Popularity),
                "popularity" => filteredProducts.OrderByDescending(product => product.Popularity).ThenByDescending(product => product.Rating),
                _ => filteredProducts.OrderByDescending(product => product.IsFeatured).ThenByDescending(product => product.Popularity)
            };

            var productViews = filteredProducts
                .Select(product => MapProductView(product, categories))
                .ToList();

            var vendorProducts = _products.Where(product => product.VendorId == vendorId).ToList();
            var topCategory = vendorProducts
                .GroupBy(product => product.CategoryId)
                .OrderByDescending(group => group.Count())
                .Select(group => categories.FirstOrDefault(category => category.Id == group.Key)?.Name ?? "Mixed")
                .FirstOrDefault() ?? "Mixed";

            snapshot = new StorefrontSnapshot(
                MapVendorOverview(vendor),
                categories.Select(category => new CategorySummary(
                    category.Id,
                    category.Name,
                    category.SortOrder,
                    vendorProducts.Count(product => product.CategoryId == category.Id))).ToList(),
                _campaigns.Where(campaign => campaign.VendorId == vendorId)
                    .OrderBy(campaign => campaign.Position)
                    .Select(MapCampaignView)
                    .ToList(),
                productViews,
                new StorefrontMetrics(
                    productViews.Count,
                    productViews.Count(product => product.Stock > 0),
                    productViews.Count == 0 ? 0 : decimal.Round(productViews.Average(product => product.Price), 2),
                    topCategory),
                new ActiveFilters(search, categoryId, sort, inStockOnly));

            return true;
        }
    }

    public bool TryGetAdminDashboard(string vendorId, out AdminDashboard dashboard)
    {
        lock (_syncRoot)
        {
            var vendor = _vendors.FirstOrDefault(v => v.Id == vendorId);
            if (vendor is null)
            {
                dashboard = default!;
                return false;
            }

            var categories = _categories
                .Where(category => category.VendorId == vendorId)
                .OrderBy(category => category.SortOrder)
                .ToList();

            var products = _products
                .Where(product => product.VendorId == vendorId)
                .OrderByDescending(product => product.IsFeatured)
                .ThenByDescending(product => product.Popularity)
                .ToList();

            dashboard = new AdminDashboard(
                MapVendorOverview(vendor),
                categories.Select(category => new CategorySummary(
                    category.Id,
                    category.Name,
                    category.SortOrder,
                    products.Count(product => product.CategoryId == category.Id))).ToList(),
                _campaigns.Where(campaign => campaign.VendorId == vendorId)
                    .OrderBy(campaign => campaign.Position)
                    .Select(MapCampaignView)
                    .ToList(),
                products.Select(product => MapProductView(product, categories)).ToList(),
                new AdminMetrics(
                    products.Count,
                    products.Count(product => product.Stock <= 10),
                    products.Count(product => product.IsFeatured),
                    _campaigns.Count(campaign => campaign.VendorId == vendorId)));

            return true;
        }
    }

    public OperationResult<ProductView> UpsertProduct(UpsertProductRequest request)
    {
        lock (_syncRoot)
        {
            var vendor = _vendors.FirstOrDefault(v => v.Id == request.VendorId);
            if (vendor is null)
            {
                return OperationResult<ProductView>.Failure("vendor_not_found", $"Vendor '{request.VendorId}' does not exist.");
            }

            var categories = _categories.Where(category => category.VendorId == request.VendorId).ToList();
            var category = categories.FirstOrDefault(item => item.Id == request.CategoryId);
            if (category is null)
            {
                return OperationResult<ProductView>.Failure("invalid_category", "Choose a valid category for this vendor.");
            }

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Description))
            {
                return OperationResult<ProductView>.Failure("validation_error", "Name and description are required.");
            }

            if (request.Price <= 0 || request.Stock < 0)
            {
                return OperationResult<ProductView>.Failure("validation_error", "Price must be positive and stock cannot be negative.");
            }

            var tags = NormalizeTags(request.Tags);
            if (string.IsNullOrWhiteSpace(request.ProductId))
            {
                var product = SeedProduct(
                    $"prd-{Guid.NewGuid():N}"[..12],
                    request.VendorId,
                    request.CategoryId,
                    request.Name.Trim(),
                    request.Description.Trim(),
                    request.Price,
                    request.OriginalPrice,
                    request.Stock,
                    4.4,
                    65 + Random.Shared.Next(25),
                    request.IsFeatured,
                    tags);

                _products.Add(product);
                return OperationResult<ProductView>.Success(MapProductView(product, categories));
            }

            var existing = _products.FirstOrDefault(product => product.Id == request.ProductId && product.VendorId == request.VendorId);
            if (existing is null)
            {
                return OperationResult<ProductView>.Failure("product_not_found", $"Product '{request.ProductId}' was not found.");
            }

            existing.CategoryId = request.CategoryId;
            existing.Name = request.Name.Trim();
            existing.Description = request.Description.Trim();
            existing.Price = request.Price;
            existing.OriginalPrice = request.OriginalPrice;
            existing.Stock = request.Stock;
            existing.IsFeatured = request.IsFeatured;
            existing.Tags = tags;
            existing.Popularity = Math.Min(99, existing.Popularity + 1);
            existing.ImageUrl = BuildProductImage(existing.Name, vendor.AccentTone, category.Name);

            return OperationResult<ProductView>.Success(MapProductView(existing, categories));
        }
    }

    public OperationResult<bool> DeleteProduct(string vendorId, string productId)
    {
        lock (_syncRoot)
        {
            var product = _products.FirstOrDefault(item => item.Id == productId && item.VendorId == vendorId);
            if (product is null)
            {
                return OperationResult<bool>.Failure("product_not_found", $"Product '{productId}' was not found.");
            }

            _products.Remove(product);
            return OperationResult<bool>.Success(true);
        }
    }

    public OperationResult<CampaignView> UpsertCampaign(UpsertCampaignRequest request)
    {
        lock (_syncRoot)
        {
            var vendor = _vendors.FirstOrDefault(v => v.Id == request.VendorId);
            if (vendor is null)
            {
                return OperationResult<CampaignView>.Failure("vendor_not_found", $"Vendor '{request.VendorId}' does not exist.");
            }

            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Subtitle))
            {
                return OperationResult<CampaignView>.Failure("validation_error", "Campaign title and subtitle are required.");
            }

            if (string.IsNullOrWhiteSpace(request.CampaignId))
            {
                var created = new Campaign
                {
                    Id = $"cmp-{Guid.NewGuid():N}"[..12],
                    VendorId = request.VendorId,
                    Title = request.Title.Trim(),
                    Subtitle = request.Subtitle.Trim(),
                    CallToAction = string.IsNullOrWhiteSpace(request.CallToAction) ? "Open campaign" : request.CallToAction.Trim(),
                    Position = request.Position <= 0 ? 1 : request.Position,
                    AccentTone = NormalizeAccentTone(request.AccentTone, vendor.AccentTone)
                };

                _campaigns.Add(created);
                return OperationResult<CampaignView>.Success(MapCampaignView(created));
            }

            var campaign = _campaigns.FirstOrDefault(item => item.Id == request.CampaignId && item.VendorId == request.VendorId);
            if (campaign is null)
            {
                return OperationResult<CampaignView>.Failure("campaign_not_found", $"Campaign '{request.CampaignId}' was not found.");
            }

            campaign.Title = request.Title.Trim();
            campaign.Subtitle = request.Subtitle.Trim();
            campaign.CallToAction = string.IsNullOrWhiteSpace(request.CallToAction) ? campaign.CallToAction : request.CallToAction.Trim();
            campaign.Position = request.Position <= 0 ? campaign.Position : request.Position;
            campaign.AccentTone = NormalizeAccentTone(request.AccentTone, vendor.AccentTone);

            return OperationResult<CampaignView>.Success(MapCampaignView(campaign));
        }
    }

    public OperationResult<bool> DeleteCampaign(string vendorId, string campaignId)
    {
        lock (_syncRoot)
        {
            var campaign = _campaigns.FirstOrDefault(item => item.Id == campaignId && item.VendorId == vendorId);
            if (campaign is null)
            {
                return OperationResult<bool>.Failure("campaign_not_found", $"Campaign '{campaignId}' was not found.");
            }

            _campaigns.Remove(campaign);
            return OperationResult<bool>.Success(true);
        }
    }

    public OperationResult<CartQuote> CalculateCartQuote(CartQuoteRequest request)
    {
        lock (_syncRoot)
        {
            var vendor = _vendors.FirstOrDefault(v => v.Id == request.VendorId);
            if (vendor is null)
            {
                return OperationResult<CartQuote>.Failure("vendor_not_found", $"Vendor '{request.VendorId}' does not exist.");
            }

            var normalizedItems = request.Items
                .Where(item => !string.IsNullOrWhiteSpace(item.ProductId) && item.Quantity > 0)
                .ToList();

            var quotes = new List<CartLineQuote>();
            foreach (var item in normalizedItems)
            {
                var product = _products.FirstOrDefault(candidate => candidate.Id == item.ProductId && candidate.VendorId == request.VendorId);
                if (product is null)
                {
                    return OperationResult<CartQuote>.Failure("product_not_found", $"Cart references missing product '{item.ProductId}'.");
                }

                var quantity = Math.Min(item.Quantity, Math.Max(product.Stock, 1));
                var lineTotal = decimal.Round(quantity * product.Price, 2);
                quotes.Add(new CartLineQuote(product.Id, product.Name, quantity, product.Stock, product.Price, lineTotal));
            }

            var subtotal = quotes.Sum(line => line.LineTotal);
            var delivery = subtotal == 0 ? 0 : subtotal >= 150 ? 0 : 8.90m;
            var tax = decimal.Round(subtotal * 0.18m, 2);
            var total = decimal.Round(subtotal + delivery + tax, 2);

            return OperationResult<CartQuote>.Success(new CartQuote(vendor.Id, "EUR", quotes, subtotal, delivery, tax, total));
        }
    }

    private static VendorOverview MapVendorOverview(Vendor vendor) =>
        new(
            vendor.Id,
            vendor.Name,
            vendor.Slug,
            vendor.SellerName,
            vendor.Domain,
            vendor.Tagline,
            vendor.HeroTitle,
            vendor.HeroCopy,
            vendor.UniqueCode,
            vendor.AccentTone);

    private static CampaignView MapCampaignView(Campaign campaign) =>
        new(
            campaign.Id,
            campaign.VendorId,
            campaign.Title,
            campaign.Subtitle,
            campaign.CallToAction,
            campaign.Position,
            campaign.AccentTone);

    private static ProductView MapProductView(Product product, IReadOnlyList<Category> categories)
    {
        var categoryName = categories.FirstOrDefault(category => category.Id == product.CategoryId)?.Name ?? "General";
        return new ProductView(
            product.Id,
            product.VendorId,
            product.CategoryId,
            categoryName,
            product.Name,
            product.Description,
            product.Price,
            product.OriginalPrice,
            product.Stock,
            product.Rating,
            product.Popularity,
            product.IsFeatured,
            product.ImageUrl,
            product.Tags);
    }

    private static Product SeedProduct(
        string id,
        string vendorId,
        string categoryId,
        string name,
        string description,
        decimal price,
        decimal? originalPrice,
        int stock,
        double rating,
        int popularity,
        bool isFeatured,
        List<string> tags)
    {
        var tone = vendorId == "urban-vault" ? "sunset" : "electric";
        var categoryLabel = categoryId.Split('-').Last().ToUpperInvariant();

        return new Product
        {
            Id = id,
            VendorId = vendorId,
            CategoryId = categoryId,
            Name = name,
            Description = description,
            Price = price,
            OriginalPrice = originalPrice,
            Stock = stock,
            Rating = rating,
            Popularity = popularity,
            IsFeatured = isFeatured,
            ImageUrl = BuildProductImage(name, tone, categoryLabel),
            Tags = tags
        };
    }

    private static List<string> NormalizeTags(string tags) =>
        tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(tag => tag.ToLowerInvariant())
            .Distinct()
            .ToList();

    private static string NormalizeAccentTone(string accentTone, string fallback)
    {
        var normalized = accentTone.Trim().ToLowerInvariant();
        return normalized switch
        {
            "sunset" or "graphite" or "electric" or "forest" or "cream" => normalized,
            _ => fallback
        };
    }

    private static string BuildProductImage(string name, string accentTone, string categoryLabel)
    {
        var palette = accentTone switch
        {
            "electric" => ("#1b45ff", "#76f6ff", "#edf4ff"),
            "forest" => ("#143e2f", "#7bf0b0", "#f0fff7"),
            "graphite" => ("#121212", "#808080", "#f7f7f7"),
            "cream" => ("#d7c8b4", "#fff6e8", "#1f1a16"),
            _ => ("#111111", "#f05d3e", "#fff6f0")
        };

        var safeName = System.Security.SecurityElement.Escape(name);
        var safeLabel = System.Security.SecurityElement.Escape(categoryLabel);
        var svg = $"""
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 960 1120" role="img" aria-label="{safeName}">
          <defs>
            <linearGradient id="g" x1="0%" y1="0%" x2="100%" y2="100%">
              <stop offset="0%" stop-color="{palette.Item1}" />
              <stop offset="100%" stop-color="{palette.Item2}" />
            </linearGradient>
          </defs>
          <rect width="960" height="1120" fill="url(#g)" />
          <circle cx="760" cy="180" r="190" fill="rgba(255,255,255,0.14)" />
          <circle cx="180" cy="960" r="220" fill="rgba(0,0,0,0.12)" />
          <rect x="72" y="80" width="220" height="44" rx="22" fill="rgba(255,255,255,0.18)" />
          <text x="104" y="109" font-family="Segoe UI, Arial, sans-serif" font-size="24" font-weight="700" fill="{palette.Item3}">DEMO DROP</text>
          <text x="72" y="860" font-family="Segoe UI, Arial, sans-serif" font-size="86" font-weight="800" fill="{palette.Item3}">{safeName}</text>
          <text x="72" y="925" font-family="Segoe UI, Arial, sans-serif" font-size="30" font-weight="500" fill="{palette.Item3}" opacity="0.8">{safeLabel} / MULTI-VENDOR</text>
          <rect x="72" y="972" width="240" height="4" fill="{palette.Item3}" opacity="0.7" />
        </svg>
        """;

        return $"data:image/svg+xml,{Uri.EscapeDataString(svg)}";
    }
}
