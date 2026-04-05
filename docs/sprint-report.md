# Sprint 2 Report — Artin Krasniqi
Data dorëzimit: 8 Prill 2026

---

## Çka Përfundova

### 1. Feature e Re — Search + FilterByPrice + Statistics (40 pikë)

**Search — Kërkim sipas emrit/kategorisë**
- Opsioni `2` në meny
- Rrjedha: `UI → ProductService.Search(keyword) → IRepository.GetAll() → filter LINQ`
- Case-insensitive: "LAPTOP" = "laptop" = "Laptop"
- Output shembull:
```
  Zgjidhni opsionin: 2
  Shkruaj emrin ose kategorine: laptop
  [INFO] U gjetën 1 rezultate per 'laptop':
  ID    Emri                      Cmimi     Stoku  Kategoria    StoreID
  --------------------------------------------------------------------------------
  1     Laptop Dell               €999.99      10  tech         S1
```

**FilterByPrice — Filtrim sipas çmimit**
- Opsioni `3` në meny
- Rrjedha: `UI → ProductService.FilterByPrice(min,max) → IRepository.GetAll() → filter LINQ`
- Output shembull:
```
  Zgjidhni opsionin: 3
  Cmimi minimal (€): abc
  [ERROR] Ju lutem shkruani numer valid jo-negativ (p.sh. 0 ose 10.50).

  Zgjidhni opsionin: 3
  Cmimi minimal (€): 20
  Cmimi maksimal (€): 100
  [INFO] Produktet me cmim €20.00 — €100.00:
  3     Nike Shoes                €89.99       50  fashion      S2
  4     Coffee Maker              €49.99       20  home         S2
  5     Yoga Mat                  €24.99      100  sports       S3
```

**GetStatistics — Statistika automatike**
- Opsioni `4` në meny
- Rrjedha: `UI → ProductService.GetStatistics() → IRepository.GetAll() → kalkulim LINQ`
- Output shembull:
```
  Zgjidhni opsionin: 4
  Numri i produkteve           5
  Cmimi total                  €2463.96
  Cmimi mesatar                €492.79
  Cmimi maksimal               €1299.00
  Cmimi minimal                €24.99
```

---

### 2. Error Handling i Plotë (25 pikë)

| Rasti | Gabimi pa trajtim | Mesazhi aktual |
|---|---|---|
| File mungon | `FileNotFoundException` crash | `[INFO] Skedari nuk u gjet. Po krijoj skedar te ri...` |
| Çmim "abc" | `FormatException` crash | `[ERROR] Ju lutem shkruani numer valid (p.sh. 29.99)` |
| ID 999 nuk ekziston | `NullReferenceException` crash | `[ERROR] Produkti me ID 999 nuk u gjet` |
| Emër bosh | crash | `[ERROR] Emri nuk mund te jete bosh!` |
| CSV i deformuar | crash | `[WARN] Rreshti CSV i deformuar u anashkalua` |

**Programi kurrë nuk crashon** — çdo gabim kapet me try-catch dhe shfaqet mesazh, pastaj menuja vazhdon.

---

### 3. Unit Tests — 17 Teste (20 pikë)

Projekti `ECommerce.Tests` me xUnit, `InMemoryProductRepository` (pa CSV, pa disk):

| Grupi | Teste | Çka testohet |
|---|---|---|
| Search | 4 | emri, kategoria, rast bosh, case-insensitive |
| FilterByPrice | 4 | rang valid, rang bosh, çmim negativ, max<min |
| Statistics | 4 | count, max, min, repository bosh |
| AddProduct | 3 | shtim valid, emër bosh, çmim negativ |
| DeleteProduct | 2 | fshirje e suksesshme, ID jo-ekzistuese |

```
dotnet test → 17 passed, 0 failed
```

---

## Çka Mbeti
Asgjë — të gjitha pjesët u kryen plotësisht.

## Çka Mësova
Mësova se Error Handling nuk është opsional. Një program profesional kurrë nuk duhet të shfaqë Exception direkt te useri. Gjithashtu kuptova vlerën e InMemoryRepository për teste — me të testoj logjikën e Service pa varësi nga skedarët CSV në disk.
