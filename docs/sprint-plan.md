# Sprint 2 Report — Artin Krasniqi
Data dorëzimit: 8 Prill 2026

---

## Çka Përfundova

### Feature e Re — Kërkim i Avancuar dhe Statistika ✓

**1. Search sipas emrit/kategorisë (UI → Service → Repository)**
- Opsioni `2` në meny — useri shkruan keyword dhe filtrohet lista
- Service → `Search(keyword)` → Repository → `GetAll()` → filter me LINQ
- Case-insensitive: "LAPTOP" = "laptop" = "Laptop"

**2. FilterByPrice — filtrim sipas çmimit (UI → Service → Repository)**
- Opsioni `3` në meny — useri vendos çmim minimal dhe maksimal
- Service → `FilterByPrice(min, max)` → Repository → filter me LINQ
- Validim: min nuk mund të jetë negativ, max duhet >= min

**3. GetStatistics — statistika automatike (UI → Service → Repository)**
- Opsioni `4` në meny — shfaq numrin, totalin, mesataren, max, min
- Service → `GetStatistics()` → Repository → llogarit me LINQ
- Rast bosh: kthen zero për të gjitha vlerat

---

### Error Handling i Plotë ✓

| Rasti | Gabimi i shmangur | Mesazhi i shfaqur |
|---|---|---|
| File mungon | `FileNotFoundException` | "Skedari nuk u gjet. Po krijoj skedar të ri..." |
| Input jo-numërik | `FormatException` | "Ju lutem shkruani numër valid (p.sh. 29.99)" |
| ID nuk ekziston | `NullReferenceException` | "Produkti me ID X nuk ekziston." |
| Çmim negativ | `ArgumentException` | "Çmimi duhet të jetë > 0!" |
| Emër bosh | `ArgumentException` | "Emri nuk mund të jetë bosh!" |
| CSV rresht i deformuar | parse crash | "[WARN] Rreshti CSV i deformuar u anashkalua" |

Programi **kurrë nuk crashon** — çdo gabim kapet me `try-catch` dhe shfaqet mesazh miqësor.

---

### Unit Tests — 15 Teste ✓

Projekti `ECommerce.Tests` me xUnit, 5 grupe testesh:

| Grupi | Teste | Çka testohet |
|---|---|---|
| Search | 4 | emri, kategoria, rast bosh, case-insensitive |
| FilterByPrice | 4 | rang valid, rang bosh, çmim negativ, max<min |
| Statistics | 4 | count, max, min, repository bosh |
| AddProduct | 3 | shtim valid, emër bosh, çmim negativ |
| DeleteProduct | 2 | fshirje e suksesshme, ID jo-ekzistuese |

**Të gjitha 15 testet kalojnë me sukses.**

---

### Update dhe Delete në CRUD ✓

- `MenuUpdateProduct()` — opsioni `7`, modifikon çmim/stok/emër/kategori
- `MenuDeleteProduct()` — opsioni `8`, kërkon konfirmim "po/jo" para fshirjes
- Të dy kalojnë nëpër: UI → Service → Repository → CSV

---

## Çka Mbeti

Asgjë nuk ka mbetur pa u implementuar nga plani i sprintit:
- ✓ Feature e re (Search + FilterByPrice + Statistics)
- ✓ Error Handling (file, input, ID)
- ✓ Unit Tests (15 teste, 5 grupe)
- ✓ Update dhe Delete

---

## Çka Mësova

Gjatë këtij sprinti mësova se **Error Handling** nuk është opsional — një program i mirë duhet të trajtojë çdo input të gabuar pa u mbyllur. Parimi kryesor: kurrë mos lejo që `Exception` të shfaqet direkt te useri. Gjithashtu kuptova vlerën e **Unit Tests** — ato na tregojnë menjëherë nëse një ndryshim i ri theu diçka që funksiononte.

---

## Struktura Finale e Projektit

```
E-commerce/
├── backend/
│   ├── Models/
│   │   ├── Product.cs
│   │   ├── ProductStatistics.cs   ← E RE Sprint 2
│   │   ├── Store.cs
│   │   ├── Order.cs
│   │   └── User.cs
│   ├── Data/
│   │   ├── IRepository.cs
│   │   ├── ProductFileRepository.cs   ← Error Handling shtuar
│   │   ├── StoreFileRepository.cs
│   │   ├── OrderFileRepository.cs
│   │   └── UserFileRepository.cs
│   ├── Services/
│   │   ├── ProductService.cs   ← FilterByPrice + GetStatistics shtuar
│   │   ├── StoreService.cs
│   │   ├── OrderService.cs
│   │   └── UserService.cs
│   ├── UI/
│   │   └── ConsoleMenu.cs   ← Meny e re me opsionet 2,3,4
│   ├── docs/
│   │   ├── sprint-plan.md
│   │   ├── sprint-report.md
│   │   ├── architecture.md
│   │   └── class-diagram.md
│   └── Program.cs
├── ECommerce.Tests/
│   ├── ECommerce.Tests.csproj
│   ├── InMemoryProductRepository.cs
│   └── ProductServiceTests.cs   ← 15 teste
├── frontend/
├── README.md
└── .gitignore
```
