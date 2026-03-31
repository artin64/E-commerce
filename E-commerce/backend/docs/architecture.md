# Architecture Documentation — ShopPlatform

## Project Overview

**ShopPlatform** is a multi-vendor e-commerce system built with a clean layered architecture.
The backend is implemented in **C# (.NET 8)** and the frontend in **TypeScript + React + Vite**.
The two are completely separated into independent projects that communicate via a REST API contract.

---

## Repository Structure

```
ShopPlatform/
│
├── backend/                         ← C# .NET 8 Console Application
│   ├── Models/                      ← Layer 1: Domain Models
│   │   ├── Product.cs
│   │   ├── Store.cs
│   │   ├── Order.cs
│   │   └── User.cs
│   │
│   ├── Data/                        ← Layer 2: Data Access (Repository Pattern)
│   │   ├── IRepository.cs           ← Generic interface
│   │   ├── ProductFileRepository.cs ← CSV implementation
│   │   ├── StoreFileRepository.cs
│   │   ├── OrderFileRepository.cs
│   │   └── UserFileRepository.cs
│   │
│   ├── Services/                    ← Layer 3: Business Logic
│   │   ├── ProductService.cs
│   │   ├── StoreService.cs
│   │   ├── OrderService.cs
│   │   └── UserService.cs
│   │
│   ├── UI/                          ← Layer 4: Presentation (Console)
│   │   └── ConsoleMenu.cs
│   │
│   ├── docs/                        ← Documentation
│   │   ├── architecture.md          ← this file
│   │   └── class-diagram.md         ← UML class diagram
│   │
│   ├── Data/                        ← CSV data files (auto-created at runtime)
│   │   ├── products.csv
│   │   ├── stores.csv
│   │   ├── orders.csv
│   │   └── users.csv
│   │
│   ├── Program.cs                   ← Entry point (max 10 lines)
│   └── ShopPlatform.csproj
│
└── frontend/                        ← React + TypeScript + Vite SPA
    ├── src/
    │   ├── models/
    │   │   └── types.ts             ← TypeScript interfaces (mirrors C# Models)
    │   ├── services/
    │   │   └── api.ts               ← API service layer (mirrors Repository Pattern)
    │   ├── components/              ← Reusable UI components
    │   └── pages/                   ← Page-level components
    ├── public/
    └── package.json
```

---

## Architecture: 4-Layer Separation

```
┌──────────────────────────────────────────────────────────┐
│                        UI LAYER                          │
│              ConsoleMenu  (UI/)                          │
│   Handles all console I/O. Never touches Data directly.  │
└─────────────────────────┬────────────────────────────────┘
                          │ calls
┌─────────────────────────▼────────────────────────────────┐
│                    SERVICES LAYER                        │
│   ProductService │ StoreService │ OrderService │         │
│   UserService    (Services/)                             │
│   Contains ALL business rules, validation, and logic.    │
└─────────────────────────┬────────────────────────────────┘
                          │ depends on (via interface)
┌─────────────────────────▼────────────────────────────────┐
│                  DATA ACCESS LAYER                       │
│   IRepository<T>  (interface)                           │
│   ProductFileRepository │ StoreFileRepository │          │
│   OrderFileRepository   │ UserFileRepository   (Data/)   │
│   Reads/writes CSV files. Isolated from business logic.  │
└─────────────────────────┬────────────────────────────────┘
                          │ maps to/from
┌─────────────────────────▼────────────────────────────────┐
│                    MODELS LAYER                          │
│     Product │ Store │ Order │ User   (Models/)           │
│   Pure domain classes. No dependencies on other layers.  │
└──────────────────────────────────────────────────────────┘
```

### Data Flow Example — Place an Order

```
ConsoleMenu.PlaceOrderMenu()
    → OrderService.PlaceOrder(buyerId, storeId, productId, qty)
        → IRepository<Product>.GetById(productId)        ← ProductFileRepository reads CSV
        → IRepository<Product>.Update(product)           ← reduces stock
        → IRepository<Product>.Save()                    ← writes products.csv
        → IRepository<Order>.Add(order)                  ← new Order object
        → IRepository<Order>.Save()                      ← writes orders.csv
    ← returns Order
← ConsoleMenu displays result
```

---

## Layer Responsibilities

### Layer 1 — Models (`Models/`)

**Responsibility:** Define the shape of domain entities. Nothing else.

- Contains only data fields (private attributes), properties, constructors.
- Each model has `ToCsv()` and `FromCsv()` methods for serialization.
- **Zero dependencies** on any other layer. Models are the foundation.

**Classes:** `Product`, `Store`, `Order`, `User`, `OrderStatus` (enum), `UserRole` (enum)

---

### Layer 2 — Data Access (`Data/`)

**Responsibility:** Read from and write to storage. Nothing else.

- `IRepository<T>` defines the contract: `GetAll()`, `GetById()`, `Add()`, `Update()`, `Delete()`, `Save()`.
- Each `FileRepository` implements `IRepository<T>` using CSV files as storage.
- Uses an **in-memory cache** (`List<T>`) loaded at startup; `Save()` flushes cache to disk.
- Storage technology is completely hidden behind the interface.

**Key pattern:** Repository Pattern — data access is abstracted away from all business logic.

---

### Layer 3 — Services (`Services/`)

**Responsibility:** Business rules and application logic. Nothing else.

- Services **never** know about CSV files or console output.
- Services depend on `IRepository<T>` — not on any concrete `FileRepository`.
- All validation, calculations, and domain operations live here.
- Example: `OrderService.PlaceOrder()` validates stock, calculates totals, coordinates two repositories.

**Key pattern:** Dependency Injection — repositories are injected into service constructors.

---

### Layer 4 — UI (`UI/`)

**Responsibility:** User interaction and display. Nothing else.

- `ConsoleMenu` handles all input/output.
- Calls only Service methods — never Data or Model methods directly.
- Catches exceptions from Services and displays user-friendly messages.
- Could be replaced with a Web API controller or GUI with zero changes to other layers.

---

## Frontend Architecture (`frontend/`)

The frontend is a separate, standalone React + TypeScript SPA:

| Folder | Purpose |
|---|---|
| `src/models/` | TypeScript interfaces mirroring backend C# models |
| `src/services/` | API service layer — mirrors Repository Pattern for the frontend |
| `src/components/` | Reusable React components |
| `src/pages/` | Page-level components (routes) |

The frontend communicates with the backend exclusively via REST API.
There is **no shared code** between frontend and backend — the contract is the API.

---

## SOLID Principles Applied

### S — Single Responsibility Principle
Each class has exactly one reason to change:
- `Product.cs` changes only if the product domain changes.
- `ProductFileRepository.cs` changes only if CSV storage format changes.
- `ProductService.cs` changes only if product business rules change.
- `ConsoleMenu.cs` changes only if the console UI changes.

### O — Open/Closed Principle
`FileRepository` classes are **closed for modification**.
To add a new storage backend (e.g., SQLite), create a new class implementing `IRepository<T>` — **no existing code changes**.

```csharp
// New backend without touching any existing class:
public class ProductSqliteRepository : IRepository<Product> { ... }
```

### L — Liskov Substitution Principle
Every `FileRepository` is a drop-in replacement for `IRepository<T>`.
`ProductService` works identically whether it receives a `ProductFileRepository`,
a `ProductSqliteRepository`, or a mock in tests.

### I — Interface Segregation Principle
`IRepository<T>` is focused and minimal — only the 6 operations needed: `GetAll`, `GetById`, `Add`, `Update`, `Delete`, `Save`.
No class is forced to implement methods it doesn't need.

### D — Dependency Inversion Principle
Services depend on the **abstraction** (`IRepository<T>`), not on the **concretion** (`ProductFileRepository`).
This is achieved via constructor injection in `Program.cs`:

```csharp
// Program.cs injects concrete implementations into services
new ProductService(new ProductFileRepository())
// ProductService only knows about IRepository<Product>
```

---

## Architecture Decisions & Reasoning

| Decision | Reasoning |
|---|---|
| **4-layer separation** | Each layer has a single responsibility; changes in one layer don't cascade to others |
| **Repository Pattern** | Decouples business logic from storage; allows swapping CSV → DB with zero service changes |
| **Generic `IRepository<T>`** | Avoids duplication; one interface covers all entity types |
| **CSV file storage** | Simple, human-readable, zero infrastructure required; easily replaced via the interface |
| **In-memory cache** | Avoids reading the file on every operation; `Save()` flushes only when needed |
| **Dependency Injection in Program.cs** | Keeps wiring in one place; `Program.cs` is the only place that knows concrete types |
| **Frontend/Backend separation** | Independent deployability; team can work in parallel; clear API contract |
| **TypeScript on frontend** | Type safety mirrors C# model definitions; catches errors at compile time |
| **SHA-256 password hashing** | Passwords never stored in plain text; simple and secure for this scope |
| **Minimal Program.cs (≤10 lines)** | Initialization only; all logic belongs in the appropriate layer |

---

## Dependency Direction

```
UI  →  Services  →  Data (via IRepository)  →  Models
                 ↑
         (interface boundary)
         concrete classes injected at Program.cs
```

**Rule:** Dependencies only flow **downward**. No lower layer ever imports from a higher layer.

---

## CSV File Format

| File | Format |
|---|---|
| `products.csv` | `id,name,price,stock,category,storeId` |
| `stores.csv` | `storeId,name,ownerId,isVerified,isActive` |
| `orders.csv` | `orderId,buyerId,storeId,productId,quantity,totalPrice,status,createdAt` |
| `users.csv` | `userId,name,email,passwordHash,role,createdAt` |
