# NexaMarket Commerce Cloud

NexaMarket Commerce Cloud is a demo-ready multi-vendor e-commerce vertical slice built for a live presentation. It focuses on one strong story instead of pretending that every future feature is complete: a seller can manage products and campaign cards in an isolated store, while a buyer can search, filter and update the cart with backend-calculated totals.

## What problem this demo solves

Large e-commerce projects often become confusing in demos because they try to show too many unfinished features at once. This project solves that by presenting one clear and realistic flow:

`seller studio -> product/campaign CRUD -> storefront discovery -> smart cart quote`

That flow proves:

- vendor-level data isolation
- live catalog management
- buyer-side search and filtering
- backend pricing logic for the cart

## Tech stack

- `C# / ASP.NET Core 10` for the backend API and static hosting
- `TypeScript` source in [frontend/app.ts](frontend/app.ts)
- prebuilt browser bundle in [wwwroot/assets/app.js](wwwroot/assets/app.js)
- modern responsive CSS in [wwwroot/assets/app.css](wwwroot/assets/app.css)

## Live features in this version

- switch between two vendors without mixing data
- ASOS-inspired storefront layout with editorial hero and promo cards
- search by name, description or tags
- filter by category, stock and sort mode
- add to cart and recalculate subtotal, delivery and tax from the backend
- seller studio for:
  - create/update/delete products
  - create/update/delete campaign cards
  - read dashboard metrics

## Project structure

- [Program.cs](Program.cs): minimal API routes and app startup
- [DemoDataStore.cs](DemoDataStore.cs): seeded multi-vendor data and business logic
- [Models.cs](Models.cs): contracts, DTOs and domain models
- [wwwroot/index.html](wwwroot/index.html): app shell
- [docs/demo-plan.md](docs/demo-plan.md): live demo plan for class presentation

## Run locally

PowerShell:

```powershell
dotnet run
```

Then open the local URL shown in the terminal.

## Recommended live demo flow

1. Start on the storefront for `Urban Vault`.
2. Search for a product and apply one category filter.
3. Add two items to the cart and show that totals update automatically.
4. Switch to `Seller Studio`.
5. Edit one product title or stock value.
6. Return to `Storefront` and show the updated result live.
7. Switch vendor to prove isolation.

## Plan B for presentation

If something breaks during the live demo:

- use [docs/demo-plan.md](docs/demo-plan.md) as your speaking structure
- show the seller studio first, then storefront
- keep one prepared search term: `runner`
- keep one prepared category: `Outerwear`
- explain that data is seeded in-memory for presentation reliability

## Current limitations

- no persistent database yet
- no real authentication or payment gateway
- no file upload for product images
- theme marketplace and plugin store are still roadmap items

That is intentional for this milestone: the app is optimized for a clean, professional live demo next week.
