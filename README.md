# Multi-Vendor E-Commerce Platform

Platformë e-commerce me arkitekturë 4-shtresore që lejon menaxhimin e plotë të produkteve, dyqaneve, porosive dhe përdoruesve.

## Tech Stack

- **Backend:** C# / ASP.NET Core .NET 8
- **Frontend:** React + TypeScript + Vite
- **Testing:** xUnit (17 unit teste)
- **Architecture:** 4-Layer (Models → Data → Services → UI)
- **Patterns:** Repository Pattern, SOLID Principles

## Arkitektura

```
Models      →   kontraktat dhe entitetet e domainit
Data        →   repository interfaces dhe implementimet
Services    →   logjika e biznesit
UI          →   React frontend + Console menu
```

## Funksionaliteti i implementuar

- CRUD i plotë për Products, Stores, Orders dhe Users
- Repository Pattern me `IProductRepository` dhe InMemory implementim
- Kërkim dhe filtrim i produkteve
- Statistika të shitjeve dhe aktivitetit
- Trajtim i gabimeve dhe validim në çdo shtresë
- 17 unit teste xUnit të izoluara

## Struktura e projektit

```
E-commerce/
├── Models/                  # Entitetet e domainit
├── Data/                    # Repository interfaces dhe implementime
├── Services/                # Logjika e biznesit
├── UI/
│   ├── ConsoleMenu.cs       # Console interface me CRUD të plotë
│   └── frontend/            # React + TypeScript app
├── Tests/                   # 17 unit teste xUnit
└── docs/
    └── demo-plan.md         # Plani i demo-s live
```

## Si ta ekzekutosh

### Backend

```bash
dotnet run
```

### Frontend

```bash
cd UI/frontend
npm install
npm run dev
```

### Testet

```bash
dotnet test
```

## Flow kryesor i demo-s

```
Admin Panel → shto produkt → modifiko produkt → kërkim + filtër → rezultat live
```

## Limitimet aktuale

- nuk ka databazë persistente — të dhënat ruhen in-memory
- nuk ka autentikim real me role të plota
- statistikat kanë nevojë për vizualizim më të detajuar

## Demo Plan

Shiko [`docs/demo-plan.md`](docs/demo-plan.md) për planin e plotë të prezantimit live.

## Sprints

| Sprint | Rezultati | Përshkrimi |
|--------|-----------|------------|
| Sprint 1 | 85/100 | Arkitektura bazë, CRUD, Repository Pattern |
| Sprint 2 | — | Search, filter, statistika, error handling, unit teste |
