# E-commerce — Multi-Vendor Platform

Platformë e-commerce multi-vendor me arkitekturë të shtresuar të pastër.
Backend: **C# .NET 8** | Frontend: **React + TypeScript + Vite**

---

## Struktura e Projektit

```
E-commerce/
├── backend/                  ← C# .NET 8 — 4-layer architecture
│   ├── Models/               ← Layer 1: Domain entities
│   │   ├── Product.cs
│   │   ├── Store.cs
│   │   ├── Order.cs
│   │   └── User.cs
│   ├── Data/                 ← Layer 2: Repository Pattern (CSV)
│   │   ├── IRepository.cs
│   │   ├── ProductFileRepository.cs
│   │   ├── StoreFileRepository.cs
│   │   ├── OrderFileRepository.cs
│   │   └── UserFileRepository.cs
│   ├── Services/             ← Layer 3: Business Logic
│   │   ├── ProductService.cs
│   │   ├── StoreService.cs
│   │   ├── OrderService.cs
│   │   └── UserService.cs
│   ├── UI/                   ← Layer 4: Console Presentation
│   │   └── ConsoleMenu.cs
│   ├── docs/
│   │   ├── architecture.md   ← Dokumentimi i arkitekturës
│   │   └── class-diagram.md  ← UML Class Diagram
│   ├── Program.cs            ← Entry point (10 rreshta)
│   └── ECommerce.csproj
│
├── frontend/                 ← React + TypeScript + Vite SPA
│   └── src/
│       ├── models/types.ts
│       └── services/api.ts
│
├── README.md
└── .gitignore
```

---

## Arkitektura — 4 Shtresa

```
UI (ConsoleMenu)
    ↓ calls
Services (ProductService, StoreService, OrderService, UserService)
    ↓ depends on abstraction
Data — IRepository<T> ← interface
    ↑ implemented by
FileRepositories (reads/writes CSV files)
    ↓ maps to/from
Models (Product, Store, Order, User)
```

Dokumentim i plotë: [`backend/docs/architecture.md`](backend/docs/architecture.md)

UML Class Diagram: [`backend/docs/class-diagram.md`](backend/docs/class-diagram.md)

---

## Si ta ekzekutosh

### Backend
```bash
cd backend
dotnet run
```
Kërkohet: .NET 8 SDK

### Frontend
```bash
cd frontend
npm install
npm run dev
```
Kërkohet: Node.js 18+

---

## Design Patterns

| Pattern | Vendndodhja |
|---|---|
| Repository Pattern | `Data/IRepository.cs` + `*FileRepository.cs` |
| Dependency Injection | `Program.cs` injekton repo → service |
| Layered Architecture | Models → Data → Services → UI |
| SOLID (S,O,L,I,D) | Shpjeguar në `docs/architecture.md` |

---

## CSV Storage

| File | Formati |
|---|---|
| `products.csv` | id, name, price, stock, category, storeId |
| `stores.csv` | storeId, name, ownerId, isVerified, isActive |
| `orders.csv` | orderId, buyerId, storeId, productId, qty, total, status, date |
| `users.csv` | userId, name, email, passwordHash, role, createdAt |
