type ViewMode = "storefront" | "studio";
type Tone = "sunset" | "graphite" | "electric" | "forest" | "cream";

interface VendorOverview {
    id: string;
    name: string;
    slug: string;
    sellerName: string;
    domain: string;
    tagline: string;
    heroTitle: string;
    heroCopy: string;
    uniqueCode: string;
    accentTone: Tone;
}

interface PlatformBootstrap {
    appName: string;
    summary: string;
    defaultVendorId: string;
    vendors: VendorOverview[];
}

interface CategorySummary {
    id: string;
    name: string;
    sortOrder: number;
    productCount: number;
}

interface CampaignView {
    id: string;
    vendorId: string;
    title: string;
    subtitle: string;
    callToAction: string;
    position: number;
    accentTone: Tone;
}

interface ProductView {
    id: string;
    vendorId: string;
    categoryId: string;
    categoryName: string;
    name: string;
    description: string;
    price: number;
    originalPrice: number | null;
    stock: number;
    rating: number;
    popularity: number;
    isFeatured: boolean;
    imageUrl: string;
    tags: string[];
}

interface StorefrontMetrics {
    totalProducts: number;
    inStockProducts: number;
    averagePrice: number;
    topCategory: string;
}

interface ActiveFilters {
    search: string;
    categoryId: string;
    sort: string;
    inStockOnly: boolean;
}

interface StorefrontSnapshot {
    vendor: VendorOverview;
    categories: CategorySummary[];
    campaigns: CampaignView[];
    products: ProductView[];
    metrics: StorefrontMetrics;
    activeFilters: ActiveFilters;
}

interface AdminMetrics {
    skuCount: number;
    lowStockCount: number;
    featuredCount: number;
    campaignCount: number;
}

interface AdminDashboard {
    vendor: VendorOverview;
    categories: CategorySummary[];
    campaigns: CampaignView[];
    products: ProductView[];
    metrics: AdminMetrics;
}

interface CartLine {
    productId: string;
    quantity: number;
}

interface CartLineQuote {
    productId: string;
    name: string;
    quantity: number;
    availableStock: number;
    unitPrice: number;
    lineTotal: number;
}

interface CartQuote {
    vendorId: string;
    currency: string;
    items: CartLineQuote[];
    subtotal: number;
    delivery: number;
    tax: number;
    total: number;
}

interface ApiError {
    code: string;
    message: string;
}

interface AppState {
    bootstrap: PlatformBootstrap | null;
    storefront: StorefrontSnapshot | null;
    admin: AdminDashboard | null;
    vendorId: string;
    view: ViewMode;
    filters: ActiveFilters;
    cart: CartLine[];
    quote: CartQuote;
    editingProductId: string | null;
    editingCampaignId: string | null;
    flash: { type: "info" | "error"; message: string } | null;
}

const state: AppState = {
    bootstrap: null,
    storefront: null,
    admin: null,
    vendorId: "",
    view: "storefront",
    filters: { search: "", categoryId: "all", sort: "featured", inStockOnly: false },
    cart: [],
    quote: createEmptyQuote(""),
    editingProductId: null,
    editingCampaignId: null,
    flash: null
};

const app = document.querySelector<HTMLDivElement>("#app");

if (!app) {
    throw new Error("App container was not found.");
}

void init();

async function init(): Promise<void> {
    renderLoading("Loading the storefront and seller studio...");

    try {
        state.bootstrap = await fetchJson<PlatformBootstrap>("/api/bootstrap");
        state.vendorId = state.bootstrap.defaultVendorId;
        await reloadCurrentVendor();
        render();
    } catch (error) {
        showFlash("error", `Unable to start the demo: ${toErrorMessage(error)}`);
        render();
    }
}

async function reloadCurrentVendor(): Promise<void> {
    const params = new URLSearchParams({
        vendorId: state.vendorId,
        search: state.filters.search,
        categoryId: state.filters.categoryId,
        sort: state.filters.sort,
        inStockOnly: String(state.filters.inStockOnly)
    });

    const [storefront, admin] = await Promise.all([
        fetchJson<StorefrontSnapshot>(`/api/storefront?${params.toString()}`),
        fetchJson<AdminDashboard>(`/api/admin/dashboard?vendorId=${encodeURIComponent(state.vendorId)}`)
    ]);

    state.storefront = storefront;
    state.admin = admin;
    await refreshQuote();
}

async function refreshQuote(): Promise<void> {
    if (state.cart.length === 0) {
        state.quote = createEmptyQuote(state.vendorId);
        return;
    }

    const quote = await fetchJson<CartQuote>("/api/cart/quote", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            vendorId: state.vendorId,
            items: state.cart
        })
    });

    state.quote = quote;
    state.cart = quote.items.map((item) => ({
        productId: item.productId,
        quantity: item.quantity
    }));
}

function render(): void {
    if (!state.bootstrap || !state.storefront || !state.admin) {
        renderLoading(state.flash?.message ?? "Preparing the demo experience...");
        return;
    }

    const editingProduct = getEditingProduct();
    const editingCampaign = getEditingCampaign();

    app.innerHTML = `
        <div class="page-shell">
            <header class="topbar">
                <div class="topbar__brand">
                    <button class="burger interactive" type="button" aria-label="Navigation menu">☰</button>
                    <div class="brand-lockup">
                        <span class="brand-eyebrow">Live demo build</span>
                        <span class="brand-name">${escapeHtml(state.bootstrap.appName)}</span>
                    </div>
                </div>
                <div class="topbar__actions">
                    <div class="pill-nav">
                        <button type="button" data-view="storefront" class="${state.view === "storefront" ? "is-active" : ""}">Storefront</button>
                        <button type="button" data-view="studio" class="${state.view === "studio" ? "is-active" : ""}">Seller Studio</button>
                    </div>
                    <span class="status-chip">Demo-ready vertical slice</span>
                </div>
            </header>

            ${renderFlash()}

            <section class="hero">
                <article class="panel hero__main">
                    <div class="eyebrow"><strong>Multi-vendor</strong> One engine, isolated stores</div>
                    <h1>${escapeHtml(state.storefront.vendor.name)}</h1>
                    <p>${escapeHtml(state.storefront.vendor.heroTitle)} ${escapeHtml(state.storefront.vendor.heroCopy)}</p>

                    <div class="hero-actions">
                        <button type="button" class="primary-button interactive" data-jump="products">Open live flow</button>
                        <button type="button" class="ghost-button interactive" data-view="studio">Go to seller studio</button>
                    </div>

                    <div class="vendor-switcher">
                        ${state.bootstrap.vendors.map((vendor) => `
                            <button
                                type="button"
                                class="vendor-chip interactive ${vendor.id === state.vendorId ? "is-active" : ""}"
                                data-vendor-id="${escapeAttribute(vendor.id)}"
                            >
                                ${escapeHtml(vendor.name)}
                            </button>
                        `).join("")}
                    </div>

                    <div class="step-rail">
                        <div class="step-pill">
                            <strong>1. Seller manages data</strong>
                            <span>CRUD products and campaign cards from one dashboard.</span>
                        </div>
                        <div class="step-pill">
                            <strong>2. Buyer filters live</strong>
                            <span>Search, stock filters and category-based discovery.</span>
                        </div>
                        <div class="step-pill">
                            <strong>3. Cart totals update</strong>
                            <span>Backend quote recalculates subtotal, delivery and tax.</span>
                        </div>
                    </div>

                    <div class="stats-grid">
                        <div class="metric-card">
                            <strong>${state.storefront.metrics.totalProducts}</strong>
                            <span>Filtered products live</span>
                        </div>
                        <div class="metric-card">
                            <strong>${state.admin.metrics.lowStockCount}</strong>
                            <span>Low-stock alerts</span>
                        </div>
                        <div class="metric-card">
                            <strong>${state.storefront.metrics.topCategory}</strong>
                            <span>Top-performing category</span>
                        </div>
                        <div class="metric-card">
                            <strong>${formatCurrency(state.quote.total || state.storefront.metrics.averagePrice)}</strong>
                            <span>Total cart or average price snapshot</span>
                        </div>
                    </div>
                </article>

                <aside class="panel hero__aside">
                    <span class="section-label">Demo Narrative</span>
                    <div class="domain-card">
                        <div class="muted-copy">Primary problem solved</div>
                        <code>Seller changes should appear instantly without mixing vendor data.</code>
                    </div>
                    <div class="domain-card">
                        <div class="muted-copy">Seller owner</div>
                        <code>${escapeHtml(state.storefront.vendor.sellerName)}</code>
                    </div>
                    <div class="domain-card">
                        <div class="muted-copy">Store domain & unique code</div>
                        <code>${escapeHtml(state.storefront.vendor.domain)}</code>
                        <code>${escapeHtml(state.storefront.vendor.uniqueCode)}</code>
                    </div>
                    <div class="insight-pills">
                        <span class="tag">Admin panel</span>
                        <span class="tag">Search + filters</span>
                        <span class="tag">Responsive UI</span>
                        <span class="tag">In-memory API</span>
                    </div>
                </aside>
            </section>

            <section class="campaigns">
                <div class="section-header">
                    <div>
                        <h2>Editorial Promotions</h2>
                        <p>These cards imitate an ASOS-style commerce rhythm: big seasonal hooks, focused storytelling and a fast path into product exploration.</p>
                    </div>
                    <button type="button" class="ghost-button interactive" data-view="studio">Edit campaigns</button>
                </div>
                <div class="campaign-grid">
                    ${state.storefront.campaigns.map(renderCampaignCard).join("")}
                </div>
            </section>

            <section class="${state.view === "storefront" ? "" : "visually-hidden"}">
                <div class="content-grid" id="products">
                    <div class="store-card">
                        <div class="section-header">
                            <div>
                                <h2>Store Discovery</h2>
                                <p>${escapeHtml(state.storefront.vendor.tagline)}</p>
                            </div>
                            <button type="button" class="ghost-button interactive" data-clear-filters>Reset filters</button>
                        </div>

                        <form id="filter-form">
                            <div class="filter-grid">
                                <div class="field">
                                    <label for="search">Search products</label>
                                    <input id="search" name="search" type="text" value="${escapeAttribute(state.filters.search)}" placeholder="Search by name, description or tag" />
                                </div>
                                <div class="field">
                                    <label for="categoryId">Category</label>
                                    <select id="categoryId" name="categoryId">
                                        <option value="all">All categories</option>
                                        ${state.storefront.categories.map((category) => `
                                            <option value="${escapeAttribute(category.id)}" ${state.filters.categoryId === category.id ? "selected" : ""}>
                                                ${escapeHtml(category.name)}
                                            </option>
                                        `).join("")}
                                    </select>
                                </div>
                                <div class="field">
                                    <label for="sort">Sort by</label>
                                    <select id="sort" name="sort">
                                        ${renderSortOption("featured", "Featured")}
                                        ${renderSortOption("popularity", "Popularity")}
                                        ${renderSortOption("rating", "Rating")}
                                        ${renderSortOption("price-asc", "Price low to high")}
                                        ${renderSortOption("price-desc", "Price high to low")}
                                    </select>
                                </div>
                                <label class="checkbox-field">
                                    <input name="inStockOnly" type="checkbox" ${state.filters.inStockOnly ? "checked" : ""} />
                                    <span>Show only products in stock</span>
                                </label>
                            </div>
                            <div class="admin-actions">
                                <button type="submit" class="primary-button interactive">Apply filters</button>
                            </div>
                        </form>

                        <div class="category-rail">
                            ${state.storefront.categories.map((category) => `
                                <span class="category-pill">${escapeHtml(category.name)} · ${category.productCount}</span>
                            `).join("")}
                        </div>

                        <div class="results-bar">
                            <p class="muted-copy">
                                Showing <strong>${state.storefront.metrics.totalProducts}</strong> products,
                                <strong>${state.storefront.metrics.inStockProducts}</strong> available right now.
                            </p>
                            <p class="muted-copy">Average price: <strong>${formatCurrency(state.storefront.metrics.averagePrice)}</strong></p>
                        </div>

                        <div class="product-grid">
                            ${state.storefront.products.length > 0
                                ? state.storefront.products.map(renderProductCard).join("")
                                : `<div class="empty-state">No products matched this search. Reset the filters or switch to another vendor.</div>`}
                        </div>
                    </div>

                    <aside class="cart-card">
                        <div class="section-header">
                            <div>
                                <h2>Smart Cart</h2>
                                <p>Totals are recalculated from the backend every time quantity changes.</p>
                            </div>
                        </div>
                        <div class="cart-list">
                            ${state.quote.items.length > 0
                                ? state.quote.items.map(renderCartItem).join("")
                                : `<div class="empty-state">Add a product to show the main buyer flow for the demo.</div>`}
                        </div>
                        <div class="cart-total">
                            <div class="cart-row"><span>Subtotal</span><span>${formatCurrency(state.quote.subtotal)}</span></div>
                            <div class="cart-row"><span>Delivery</span><span>${state.quote.delivery === 0 ? "Free" : formatCurrency(state.quote.delivery)}</span></div>
                            <div class="cart-row"><span>Tax</span><span>${formatCurrency(state.quote.tax)}</span></div>
                            <div class="cart-row"><strong>Total</strong><strong>${formatCurrency(state.quote.total)}</strong></div>
                        </div>
                    </aside>
                </div>
            </section>

            <section class="${state.view === "studio" ? "" : "visually-hidden"}">
                <div class="section-header">
                    <div>
                        <h2>Seller Studio</h2>
                        <p>Use this screen live during the presentation to prove that each seller can manage their own catalog and promotions independently.</p>
                    </div>
                </div>

                <div class="studio-grid">
                    <div class="admin-column">
                        <div class="panel admin-column">
                            <div class="section-header">
                                <div>
                                    <h2>${editingProduct ? "Edit Product" : "Add Product"}</h2>
                                    <p>Changes here update the same storefront the buyer sees.</p>
                                </div>
                            </div>
                            <form id="product-form">
                                <input type="hidden" name="productId" value="${escapeAttribute(editingProduct?.id ?? "")}" />
                                <div class="form-grid">
                                    <div class="field">
                                        <label for="product-name">Name</label>
                                        <input id="product-name" name="name" type="text" required value="${escapeAttribute(editingProduct?.name ?? "")}" />
                                    </div>
                                    <div class="field">
                                        <label for="product-category">Category</label>
                                        <select id="product-category" name="categoryId" required>
                                            ${state.admin.categories.map((category) => `
                                                <option value="${escapeAttribute(category.id)}" ${(editingProduct?.categoryId ?? state.admin.categories[0]?.id) === category.id ? "selected" : ""}>
                                                    ${escapeHtml(category.name)}
                                                </option>
                                            `).join("")}
                                        </select>
                                    </div>
                                    <div class="field field--full">
                                        <label for="product-description">Description</label>
                                        <textarea id="product-description" name="description" required>${escapeHtml(editingProduct?.description ?? "")}</textarea>
                                    </div>
                                    <div class="field">
                                        <label for="product-price">Price</label>
                                        <input id="product-price" name="price" type="number" min="1" step="0.01" required value="${editingProduct?.price ?? ""}" />
                                    </div>
                                    <div class="field">
                                        <label for="product-original-price">Original price</label>
                                        <input id="product-original-price" name="originalPrice" type="number" min="0" step="0.01" value="${editingProduct?.originalPrice ?? ""}" />
                                    </div>
                                    <div class="field">
                                        <label for="product-stock">Stock</label>
                                        <input id="product-stock" name="stock" type="number" min="0" step="1" required value="${editingProduct?.stock ?? 10}" />
                                    </div>
                                    <div class="field">
                                        <label for="product-tags">Tags</label>
                                        <input id="product-tags" name="tags" type="text" placeholder="new-in, travel, featured" value="${escapeAttribute(editingProduct?.tags.join(", ") ?? "")}" />
                                    </div>
                                </div>
                                <label class="checkbox-field">
                                    <input name="isFeatured" type="checkbox" ${editingProduct?.isFeatured ? "checked" : ""} />
                                    <span>Feature this product in ranking</span>
                                </label>
                                <div class="admin-actions">
                                    <button type="submit" class="primary-button interactive">${editingProduct ? "Update product" : "Create product"}</button>
                                    <button type="button" class="ghost-button interactive" data-reset-product-form>Clear form</button>
                                </div>
                            </form>
                        </div>

                        <div class="panel admin-column">
                            <div class="section-header">
                                <div>
                                    <h2>${editingCampaign ? "Edit Campaign" : "Add Campaign"}</h2>
                                    <p>Use bold messaging and ordering to control the storefront rhythm.</p>
                                </div>
                            </div>
                            <form id="campaign-form">
                                <input type="hidden" name="campaignId" value="${escapeAttribute(editingCampaign?.id ?? "")}" />
                                <div class="form-grid">
                                    <div class="field">
                                        <label for="campaign-title">Title</label>
                                        <input id="campaign-title" name="title" type="text" required value="${escapeAttribute(editingCampaign?.title ?? "")}" />
                                    </div>
                                    <div class="field">
                                        <label for="campaign-position">Position</label>
                                        <input id="campaign-position" name="position" type="number" min="1" step="1" required value="${editingCampaign?.position ?? 1}" />
                                    </div>
                                    <div class="field field--full">
                                        <label for="campaign-subtitle">Subtitle</label>
                                        <textarea id="campaign-subtitle" name="subtitle" required>${escapeHtml(editingCampaign?.subtitle ?? "")}</textarea>
                                    </div>
                                    <div class="field">
                                        <label for="campaign-cta">Call to action</label>
                                        <input id="campaign-cta" name="callToAction" type="text" value="${escapeAttribute(editingCampaign?.callToAction ?? "Open campaign")}" />
                                    </div>
                                    <div class="field">
                                        <label for="campaign-tone">Accent tone</label>
                                        <select id="campaign-tone" name="accentTone">
                                            ${["sunset", "graphite", "electric", "forest", "cream"].map((tone) => `
                                                <option value="${tone}" ${(editingCampaign?.accentTone ?? state.admin.vendor.accentTone) === tone ? "selected" : ""}>${tone}</option>
                                            `).join("")}
                                        </select>
                                    </div>
                                </div>
                                <div class="admin-actions">
                                    <button type="submit" class="primary-button interactive">${editingCampaign ? "Update campaign" : "Create campaign"}</button>
                                    <button type="button" class="ghost-button interactive" data-reset-campaign-form>Clear form</button>
                                </div>
                            </form>
                        </div>
                    </div>

                    <div class="admin-column">
                        <div class="panel admin-column">
                            <div class="section-header">
                                <div>
                                    <h2>Dashboard Snapshot</h2>
                                    <p>These metrics help you explain why the chosen flow matters for the seller side.</p>
                                </div>
                            </div>
                            <div class="stats-grid">
                                <div class="metric-card">
                                    <strong>${state.admin.metrics.skuCount}</strong>
                                    <span>Total SKUs</span>
                                </div>
                                <div class="metric-card">
                                    <strong>${state.admin.metrics.featuredCount}</strong>
                                    <span>Featured products</span>
                                </div>
                                <div class="metric-card">
                                    <strong>${state.admin.metrics.lowStockCount}</strong>
                                    <span>Need replenishment</span>
                                </div>
                                <div class="metric-card">
                                    <strong>${state.admin.metrics.campaignCount}</strong>
                                    <span>Live campaign cards</span>
                                </div>
                            </div>
                        </div>

                        <div class="admin-split">
                            <div class="panel admin-column">
                                <div class="section-header">
                                    <div>
                                        <h2>Catalog</h2>
                                        <p>Use edit/delete actions live to demonstrate CRUD.</p>
                                    </div>
                                </div>
                                <div class="list-stack">
                                    ${state.admin.products.length > 0
                                        ? state.admin.products.map(renderAdminProductCard).join("")
                                        : `<div class="empty-state">No products yet for this vendor.</div>`}
                                </div>
                            </div>

                            <div class="panel admin-column">
                                <div class="section-header">
                                    <div>
                                        <h2>Campaigns</h2>
                                        <p>Promotional blocks stay isolated per store.</p>
                                    </div>
                                </div>
                                <div class="list-stack">
                                    ${state.admin.campaigns.length > 0
                                        ? state.admin.campaigns.map(renderAdminCampaignCard).join("")
                                        : `<div class="empty-state">No campaign cards created for this vendor.</div>`}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    `;

    bindEvents();
}

function bindEvents(): void {
    document.querySelectorAll<HTMLElement>("[data-view]").forEach((button) => {
        button.addEventListener("click", async () => {
            const view = button.dataset.view as ViewMode;
            state.view = view;
            render();
        });
    });

    document.querySelectorAll<HTMLElement>("[data-vendor-id]").forEach((button) => {
        button.addEventListener("click", async () => {
            const vendorId = button.dataset.vendorId;
            if (!vendorId || vendorId === state.vendorId) {
                return;
            }

            state.vendorId = vendorId;
            state.filters = { ...state.filters, search: "", categoryId: "all", inStockOnly: false };
            state.cart = [];
            state.quote = createEmptyQuote(vendorId);
            state.editingProductId = null;
            state.editingCampaignId = null;
            showFlash("info", "Switched vendor. Cart reset to preserve store isolation.");
            await reloadCurrentVendor();
            render();
        });
    });

    document.querySelectorAll<HTMLElement>("[data-jump='products']").forEach((button) => {
        button.addEventListener("click", () => {
            document.getElementById("products")?.scrollIntoView({ behavior: "smooth", block: "start" });
        });
    });

    const filterForm = document.querySelector<HTMLFormElement>("#filter-form");
    filterForm?.addEventListener("submit", async (event) => {
        event.preventDefault();
        const formData = new FormData(filterForm);
        state.filters = {
            search: String(formData.get("search") ?? "").trim(),
            categoryId: String(formData.get("categoryId") ?? "all"),
            sort: String(formData.get("sort") ?? "featured"),
            inStockOnly: formData.get("inStockOnly") === "on"
        };

        await reloadCurrentVendor();
        render();
    });

    document.querySelectorAll<HTMLElement>("[data-clear-filters]").forEach((button) => {
        button.addEventListener("click", async () => {
            state.filters = { search: "", categoryId: "all", sort: "featured", inStockOnly: false };
            await reloadCurrentVendor();
            render();
        });
    });

    document.querySelectorAll<HTMLElement>("[data-add-to-cart]").forEach((button) => {
        button.addEventListener("click", async () => {
            const productId = button.dataset.addToCart;
            if (!productId) {
                return;
            }

            const existing = state.cart.find((item) => item.productId === productId);
            if (existing) {
                existing.quantity += 1;
            } else {
                state.cart.push({ productId, quantity: 1 });
            }

            await refreshQuote();
            showFlash("info", "Cart updated from the live storefront.");
            render();
        });
    });

    document.querySelectorAll<HTMLElement>("[data-cart-action]").forEach((button) => {
        button.addEventListener("click", async () => {
            const action = button.dataset.cartAction;
            const productId = button.dataset.productId;
            if (!action || !productId) {
                return;
            }

            const target = state.cart.find((item) => item.productId === productId);
            if (!target) {
                return;
            }

            if (action === "decrease") {
                target.quantity -= 1;
            }

            if (action === "increase") {
                target.quantity += 1;
            }

            if (action === "remove" || target.quantity <= 0) {
                state.cart = state.cart.filter((item) => item.productId !== productId);
            }

            await refreshQuote();
            render();
        });
    });

    const productForm = document.querySelector<HTMLFormElement>("#product-form");
    productForm?.addEventListener("submit", async (event) => {
        event.preventDefault();
        const formData = new FormData(productForm);
        const productId = String(formData.get("productId") ?? "").trim();
        const payload = {
            vendorId: state.vendorId,
            productId: productId || null,
            categoryId: String(formData.get("categoryId") ?? ""),
            name: String(formData.get("name") ?? "").trim(),
            description: String(formData.get("description") ?? "").trim(),
            price: Number(formData.get("price") ?? 0),
            originalPrice: Number(formData.get("originalPrice") ?? 0) || null,
            stock: Number(formData.get("stock") ?? 0),
            isFeatured: formData.get("isFeatured") === "on",
            tags: String(formData.get("tags") ?? "")
        };

        await sendJson(productId ? `/api/admin/products/${encodeURIComponent(productId)}` : "/api/admin/products", {
            method: productId ? "PUT" : "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        state.editingProductId = null;
        showFlash("info", productId ? "Product updated successfully." : "Product created successfully.");
        await reloadCurrentVendor();
        render();
    });

    document.querySelectorAll<HTMLElement>("[data-edit-product]").forEach((button) => {
        button.addEventListener("click", () => {
            state.editingProductId = button.dataset.editProduct ?? null;
            state.view = "studio";
            render();
        });
    });

    document.querySelectorAll<HTMLElement>("[data-delete-product]").forEach((button) => {
        button.addEventListener("click", async () => {
            const productId = button.dataset.deleteProduct;
            if (!productId) {
                return;
            }

            await sendJson(`/api/admin/products/${encodeURIComponent(productId)}?vendorId=${encodeURIComponent(state.vendorId)}`, {
                method: "DELETE"
            });

            state.cart = state.cart.filter((item) => item.productId !== productId);
            state.editingProductId = null;
            showFlash("info", "Product deleted from seller studio.");
            await reloadCurrentVendor();
            render();
        });
    });

    document.querySelectorAll<HTMLElement>("[data-reset-product-form]").forEach((button) => {
        button.addEventListener("click", () => {
            state.editingProductId = null;
            render();
        });
    });

    const campaignForm = document.querySelector<HTMLFormElement>("#campaign-form");
    campaignForm?.addEventListener("submit", async (event) => {
        event.preventDefault();
        const formData = new FormData(campaignForm);
        const campaignId = String(formData.get("campaignId") ?? "").trim();
        const payload = {
            vendorId: state.vendorId,
            campaignId: campaignId || null,
            title: String(formData.get("title") ?? "").trim(),
            subtitle: String(formData.get("subtitle") ?? "").trim(),
            callToAction: String(formData.get("callToAction") ?? "").trim(),
            position: Number(formData.get("position") ?? 1),
            accentTone: String(formData.get("accentTone") ?? state.admin?.vendor.accentTone ?? "sunset")
        };

        await sendJson(campaignId ? `/api/admin/campaigns/${encodeURIComponent(campaignId)}` : "/api/admin/campaigns", {
            method: campaignId ? "PUT" : "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        state.editingCampaignId = null;
        showFlash("info", campaignId ? "Campaign updated successfully." : "Campaign created successfully.");
        await reloadCurrentVendor();
        render();
    });

    document.querySelectorAll<HTMLElement>("[data-edit-campaign]").forEach((button) => {
        button.addEventListener("click", () => {
            state.editingCampaignId = button.dataset.editCampaign ?? null;
            state.view = "studio";
            render();
        });
    });

    document.querySelectorAll<HTMLElement>("[data-delete-campaign]").forEach((button) => {
        button.addEventListener("click", async () => {
            const campaignId = button.dataset.deleteCampaign;
            if (!campaignId) {
                return;
            }

            await sendJson(`/api/admin/campaigns/${encodeURIComponent(campaignId)}?vendorId=${encodeURIComponent(state.vendorId)}`, {
                method: "DELETE"
            });

            state.editingCampaignId = null;
            showFlash("info", "Campaign deleted from the storefront.");
            await reloadCurrentVendor();
            render();
        });
    });

    document.querySelectorAll<HTMLElement>("[data-reset-campaign-form]").forEach((button) => {
        button.addEventListener("click", () => {
            state.editingCampaignId = null;
            render();
        });
    });
}

function renderCampaignCard(campaign: CampaignView): string {
    return `
        <article class="campaign-card campaign-card--${escapeAttribute(campaign.accentTone)}">
            <div>
                <span class="section-label">Position ${campaign.position}</span>
                <h3>${escapeHtml(campaign.title)}</h3>
                <p>${escapeHtml(campaign.subtitle)}</p>
            </div>
            <strong>${escapeHtml(campaign.callToAction)}</strong>
        </article>
    `;
}

function renderProductCard(product: ProductView): string {
    return `
        <article class="product-card">
            <img src="${product.imageUrl}" alt="${escapeAttribute(product.name)}" />
            <div class="product-card__body">
                <div class="product-card__meta">
                    <span>${escapeHtml(product.categoryName)}</span>
                    <span>★ ${product.rating.toFixed(1)}</span>
                </div>
                <strong class="product-card__name">${escapeHtml(product.name)}</strong>
                <div class="product-card__description">${escapeHtml(product.description)}</div>
                <div class="product-tags">
                    ${product.tags.slice(0, 3).map((tag) => `<span class="tag">${escapeHtml(tag)}</span>`).join("")}
                </div>
                <div class="price-row">
                    <div>
                        <div class="price-main">${formatCurrency(product.price)}</div>
                        ${product.originalPrice ? `<div class="price-old">${formatCurrency(product.originalPrice)}</div>` : ""}
                    </div>
                    <span class="stock-pill ${product.stock > 10 ? "stock-pill--ok" : "stock-pill--low"}">${product.stock} in stock</span>
                </div>
                <div class="admin-actions">
                    <button type="button" class="primary-button interactive" data-add-to-cart="${escapeAttribute(product.id)}">Add to cart</button>
                </div>
            </div>
        </article>
    `;
}

function renderCartItem(item: CartLineQuote): string {
    return `
        <div class="cart-item">
            <h4>${escapeHtml(item.name)}</h4>
            <p class="muted-copy">Available stock: ${item.availableStock}</p>
            <div class="cart-row">
                <span>${formatCurrency(item.unitPrice)} each</span>
                <strong>${formatCurrency(item.lineTotal)}</strong>
            </div>
            <div class="cart-actions">
                <button type="button" class="mini-button interactive" data-cart-action="decrease" data-product-id="${escapeAttribute(item.productId)}">-</button>
                <span class="tag">Qty ${item.quantity}</span>
                <button type="button" class="mini-button interactive" data-cart-action="increase" data-product-id="${escapeAttribute(item.productId)}">+</button>
                <button type="button" class="ghost-button interactive" data-cart-action="remove" data-product-id="${escapeAttribute(item.productId)}">Remove</button>
            </div>
        </div>
    `;
}

function renderAdminProductCard(product: ProductView): string {
    return `
        <article class="admin-card">
            <div class="admin-row">
                <h3 class="admin-card__title">${escapeHtml(product.name)}</h3>
                <span class="stock-pill ${product.stock > 10 ? "stock-pill--ok" : "stock-pill--low"}">${product.stock} units</span>
            </div>
            <p class="admin-card__meta">${escapeHtml(product.categoryName)} · Popularity ${product.popularity} · Rating ${product.rating.toFixed(1)}</p>
            <div class="inventory-tags">
                ${product.tags.slice(0, 4).map((tag) => `<span class="inventory-chip">${escapeHtml(tag)}</span>`).join("")}
                ${product.isFeatured ? `<span class="inventory-chip">featured</span>` : ""}
            </div>
            <div class="price-row">
                <div>
                    <div class="price-main">${formatCurrency(product.price)}</div>
                    ${product.originalPrice ? `<div class="price-old">${formatCurrency(product.originalPrice)}</div>` : ""}
                </div>
            </div>
            <div class="admin-actions">
                <button type="button" class="primary-button interactive" data-edit-product="${escapeAttribute(product.id)}">Edit</button>
                <button type="button" class="ghost-button interactive" data-delete-product="${escapeAttribute(product.id)}">Delete</button>
            </div>
        </article>
    `;
}

function renderAdminCampaignCard(campaign: CampaignView): string {
    return `
        <article class="campaign-admin-card">
            <div class="admin-row">
                <h3 class="campaign-card__title">${escapeHtml(campaign.title)}</h3>
                <span class="tag">Position ${campaign.position}</span>
            </div>
            <p class="campaign-card__copy">${escapeHtml(campaign.subtitle)}</p>
            <div class="inventory-tags">
                <span class="inventory-chip">${escapeHtml(campaign.callToAction)}</span>
                <span class="inventory-chip">${escapeHtml(campaign.accentTone)}</span>
            </div>
            <div class="campaign-actions">
                <button type="button" class="primary-button interactive" data-edit-campaign="${escapeAttribute(campaign.id)}">Edit</button>
                <button type="button" class="ghost-button interactive" data-delete-campaign="${escapeAttribute(campaign.id)}">Delete</button>
            </div>
        </article>
    `;
}

function renderSortOption(value: string, label: string): string {
    return `<option value="${value}" ${state.filters.sort === value ? "selected" : ""}>${label}</option>`;
}

function renderFlash(): string {
    if (!state.flash) {
        return "";
    }

    return `<div class="flash flash--${state.flash.type}">${escapeHtml(state.flash.message)}</div>`;
}

function getEditingProduct(): ProductView | null {
    return state.admin?.products.find((product) => product.id === state.editingProductId) ?? null;
}

function getEditingCampaign(): CampaignView | null {
    return state.admin?.campaigns.find((campaign) => campaign.id === state.editingCampaignId) ?? null;
}

function createEmptyQuote(vendorId: string): CartQuote {
    return {
        vendorId,
        currency: "EUR",
        items: [],
        subtotal: 0,
        delivery: 0,
        tax: 0,
        total: 0
    };
}

function showFlash(type: "info" | "error", message: string): void {
    state.flash = { type, message };
}

function renderLoading(message: string): void {
    app.innerHTML = `
        <div class="loading-shell">
            <div class="loading-card">
                <h1>NexaMarket</h1>
                <p>${escapeHtml(message)}</p>
            </div>
        </div>
    `;
}

async function fetchJson<T>(url: string, init?: RequestInit): Promise<T> {
    const response = await fetch(url, init);
    if (!response.ok) {
        throw await toApiError(response);
    }

    return response.json() as Promise<T>;
}

async function sendJson(url: string, init?: RequestInit): Promise<void> {
    const response = await fetch(url, init);
    if (!response.ok) {
        throw await toApiError(response);
    }
}

async function toApiError(response: Response): Promise<ApiError> {
    try {
        return await response.json() as ApiError;
    } catch {
        return { code: "unknown_error", message: `Request failed with status ${response.status}.` };
    }
}

function toErrorMessage(error: unknown): string {
    if (typeof error === "object" && error && "message" in error) {
        return String((error as { message: string }).message);
    }

    return "Unexpected error";
}

function formatCurrency(value: number): string {
    return new Intl.NumberFormat("en-GB", {
        style: "currency",
        currency: "EUR"
    }).format(value);
}

function escapeHtml(value: string): string {
    return value
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll("\"", "&quot;")
        .replaceAll("'", "&#39;");
}

function escapeAttribute(value: string): string {
    return escapeHtml(value);
}
