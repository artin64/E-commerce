# Implementation Documentation — E-Commerce CRUD System

## Projekti: E-Commerce Multi-Vendor Platform
**GitHub:** https://github.com/artin64/E-commerce  
**Teknologjia:** C# .NET 8 · Arkitekturë 4-shtresore · CSV Storage  
**Rrjedha:** `UI → Service → Repository → CSV File`

---

## Ushtrim 1: Model + Repository

### Modeli kryesor: `Product`

```
backend/Models/Product.cs
```

**4+ atribute private me tipet e sakta:**

| Atributi | Tipi | Përshkrimi |
|---|---|---|
| `_id` | `int` | Identifikuesi unik |
| `_name` | `string` | Emri i produktit |
| `_price` | `decimal` | Çmimi (€) |
| `_stock` | `int` | Sasia në stok |
| `_category` | `string` | Kategoria |
| `_storeId` | `string` | ID e dyqanit |

### FileRepository: `ProductFileRepository`

```
backend/Data/ProductFileRepository.cs
```

**GetAll() — lexon nga CSV:**
```csharp
public IEnumerable<Product> GetAll() => _cache.AsReadOnly();

private void LoadFromFile()
{
    _cache = File.ReadAllLines(_filePath)
                 .Where(l => !string.IsNullOrWhiteSpace(l))
                 .Select(Product.FromCsv)
                 .ToList();
}
```

**GetById(id) — kërkon në cache:**
```csharp
public Product? GetById(string id)
{
    if (!int.TryParse(id, out int intId)) return null;
    return _cache.FirstOrDefault(p => p.Id == intId);
}
```

**Add(item) — shton me auto-ID:**
```csharp
public void Add(Product entity)
{
    if (entity.Id == 0)
        entity.Id = _cache.Count > 0 ? _cache.Max(p => p.Id) + 1 : 1;
    _cache.Add(entity);
}
```

**Save() — shkruan në CSV:**
```csharp
public void Save()
{
    File.WriteAllLines(_filePath, _cache.Select(p => p.ToCsv()));
}
```

**Update() — përditëson rekord:**
```csharp
public void Update(Product entity)
{
    int idx = _cache.FindIndex(p => p.Id == entity.Id);
    if (idx == -1) throw new KeyNotFoundException($"Product ID {entity.Id} not found.");
    _cache[idx] = entity;
}
```

**Delete() — fshin rekord:**
```csharp
public void Delete(string id)
{
    if (!int.TryParse(id, out int intId))
        throw new ArgumentException($"Invalid ID: {id}");
    if (_cache.RemoveAll(p => p.Id == intId) == 0)
        throw new KeyNotFoundException($"Product ID {id} not found.");
}
```

### CSV me 7 rekorde fillestare (`Data/products.csv`):

```csv
1,MacBook Pro 16,2499.99,12,Elektronike,STORE-001
2,iPhone 15 Pro Max,1399.00,34,Elektronike,STORE-001
3,Sony WH-1000XM5,349.99,23,Audio,STORE-001
4,Nike Air Max 2024,189.99,87,Kepuce,STORE-002
5,Dyson V15 Detect,699.00,8,Shtepia,STORE-002
6,Samsung OLED 65,1299.99,5,Elektronike,STORE-001
7,Adidas Ultraboost,159.99,45,Kepuce,STORE-002
```

**Format CSV:** `id,emri,çmimi,stoku,kategoria,storeId`

---

## Ushtrim 2: Service me Logjikë

```
backend/Services/ProductService.cs
```

**Dependency Injection — Repository si parameter:**
```csharp
public ProductService(IRepository<Product> repository)
{
    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
}
```

### Metoda 1: Listo me filtrim
```csharp
public IEnumerable<Product> GetAll() => _repository.GetAll();

public IEnumerable<Product> GetByCategory(string category) =>
    _repository.GetAll().Where(p =>
        p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
```

### Metoda 2: Shto me validim strikt
```csharp
public void AddProduct(Product product)
{
    Validate(product);           // ← validim para shtimit
    _repository.Add(product);
    _repository.Save();          // ← ruhet në CSV menjëherë
}

private static void Validate(Product p)
{
    if (string.IsNullOrWhiteSpace(p.Name))    
        throw new ArgumentException("Emri nuk mund të jetë bosh!");
    if (p.Price <= 0)                          
        throw new ArgumentException("Çmimi duhet të jetë > 0!");
    if (string.IsNullOrWhiteSpace(p.StoreId)) 
        throw new ArgumentException("StoreId kërkohet!");
    if (p.Stock < 0)                           
        throw new ArgumentException("Stoku nuk mund të jetë negativ!");
}
```

### Metoda 3: Gjej sipas ID
```csharp
public Product? GetById(int id) => _repository.GetById(id.ToString());
```

### Update + Delete në Service:
```csharp
public void UpdateProduct(Product product)
{
    Validate(product);
    _repository.Update(product);
    _repository.Save();
}

public void DeleteProduct(int id)
{
    _repository.Delete(id.ToString());
    _repository.Save();
}
```

---

## Ushtrim 3: UI — Console Menu

```
backend/UI/ConsoleMenu.cs
```

**Rrjedha e plotë: UI → Service → Repository → CSV File**

```
User input
    ↓
ConsoleMenu.MenuAddProduct()
    ↓
ProductService.AddProduct(product)      ← validim
    ↓
ProductFileRepository.Add(product)      ← cache
    ↓
ProductFileRepository.Save()            ← File.WriteAllLines → products.csv
```

### Output i Menusë Kryesore:

```
  ╔══════════════════════════════════════════════════╗
  ║   E-COMMERCE — CRUD System  |  .NET 8  v2.0     ║
  ║   UI → Service → Repository → CSV File          ║
  ╚══════════════════════════════════════════════════╝

╔══════════════════════════════════════════════╗
║      E-COMMERCE PLATFORM — MENU KRYESORE    ║
╠══════════════════════════════════════════════╣
║  ── PRODUKTET (CRUD) ──                     ║
║  1. Listo produktet (me filtrim kategorisë)  ║
║  2. Gjej produkt sipas ID                    ║
║  3. Shto produkt të ri (Add)                 ║
║  4. Përditëso produkt (Update)               ║
║  5. Fshi produkt (Delete)                    ║
╠══════════════════════════════════════════════╣
║  ── TJETËR ──                               ║
║  6. Listo dyqanet                            ║
║  7. Krijo dyqan                              ║
║  8. Regjistrim                               ║
║  9. Hyrje (Login)                            ║
║  0. Dil                                      ║
╚══════════════════════════════════════════════╝
```

### Output: Listim produkteve (opsioni 1)

```
  ╔─ LISTA E PRODUKTEVE ──────────────────────────────╗

  Filtro sipas kategorisë (Enter = të gjitha): 

  ID    Emri                      Çmimi     Stoku  Kategoria       StoreID
  ──────────────────────────────────────────────────────────────────────────
  1     MacBook Pro 16          €2499.99       12  Elektronike     STORE-001
  2     iPhone 15 Pro Max       €1399.00       34  Elektronike     STORE-001
  3     Sony WH-1000XM5          €349.99       23  Audio           STORE-001
  4     Nike Air Max 2024        €189.99       87  Kepuce          STORE-002
  5     Dyson V15 Detect         €699.00        8  Shtepia         STORE-002
  6     Samsung OLED 65         €1299.99        5  Elektronike     STORE-001
  7     Adidas Ultraboost        €159.99       45  Kepuce          STORE-002

  Gjithsej: 7 produkte.
```

### Output: Filtrim sipas kategorisë

```
  Filtro sipas kategorisë: Elektronike

  ID    Emri                      Çmimi     Stoku  Kategoria
  1     MacBook Pro 16          €2499.99       12  Elektronike
  2     iPhone 15 Pro Max       €1399.00       34  Elektronike
  6     Samsung OLED 65         €1299.99        5  Elektronike

  Gjithsej: 3 produkte.
```

### Output: Gjej sipas ID (opsioni 2)

```
  ╔─ GJEJ PRODUKT SIPAS ID ──────────────────────────╗
  ID e produktit: 3

  ID        : 3
  Emri      : Sony WH-1000XM5
  Çmimi     : €349.99
  Stoku     : 23 copë
  Kategoria : Audio
  Store ID  : STORE-001
  Në stok   : Po ✓
```

### Output: Shto produkt (opsioni 3) — me validim

```
  ╔─ SHTO PRODUKT TË RI ─────────────────────────────╗
  Emri i produktit: iPad Pro M4
  Çmimi (€): 1199.99
  Stoku (copë): 20
  Kategoria: Elektronike
  Store ID: STORE-001

  ✓ Produkti 'iPad Pro M4' u shtua! (ID: 8) — u ruajt në products.csv
```

**Validimi — rast me gabim:**
```
  Emri i produktit: 
  ✗ Emri nuk mund të jetë bosh!

  Çmimi (€): -5
  ✗ Çmimi duhet të jetë numër pozitiv (> 0)!
```

### Output: Update (opsioni 4)

```
  ╔─ PËRDITËSO PRODUKT (UPDATE) ─────────────────────╗
  ID e produktit: 3

  Produkti aktual: [3] Sony WH-1000XM5 | €349.99 | Stock:23 | Audio | STORE-001
  (Shtyp Enter për të mbajtur vlerën aktuale)

  Emri i ri [Sony WH-1000XM5]: Sony WH-1000XM5 (2025 Edition)
  Çmimi i ri [349.99]: 379.99
  Stoku i ri [23]: 25
  Kategoria [Audio]: 

  ✓ Produkti ID:3 u përditësua — u ruajt në products.csv
```

### Output: Delete (opsioni 5)

```
  ╔─ FSHI PRODUKT (DELETE) ──────────────────────────╗
  ID e produktit: 7

  Do të fshihet: [7] Adidas Ultraboost | €159.99 | Stock:45 | Kepuce | STORE-002
  Je i sigurt? (po/jo): po

  ✓ Produkti ID:7 u fshi — u përditësua products.csv
```

---

## Ushtrim 4 (Bonus): Update + Delete

### Implementimi i plotë Update+Delete:

**Repository (Data layer):**
```csharp
// ProductFileRepository.cs
public void Update(Product entity)
{
    int idx = _cache.FindIndex(p => p.Id == entity.Id);
    if (idx == -1) throw new KeyNotFoundException($"Product ID {entity.Id} not found.");
    _cache[idx] = entity;    // zëvendëso në cache
}

public void Delete(string id)
{
    if (_cache.RemoveAll(p => p.Id == intId) == 0)
        throw new KeyNotFoundException($"Product ID {id} not found.");
}
```

**Service (Business Logic):**
```csharp
// ProductService.cs
public void UpdateProduct(Product product)
{
    Validate(product);           // validim
    _repository.Update(product);
    _repository.Save();          // persist në CSV
}

public void DeleteProduct(int id)
{
    _repository.Delete(id.ToString());
    _repository.Save();          // persist në CSV
}
```

**UI (ConsoleMenu):**
- Opsioni `4` → `MenuUpdateProduct()` → shfaq vlerën aktuale → lexon ndryshimet → konfirmon
- Opsioni `5` → `MenuDeleteProduct()` → shfaq produktin → kërkon konfirmim `(po/jo)` → fshin

---

## Çfarë Funksionon End-to-End

| Operacioni | UI | Service | Repository | CSV |
|---|---|---|---|---|
| **Read (Listo)** | Menu opsioni 1 | `GetAll()` / `GetByCategory()` | `LoadFromFile()` | `products.csv` |
| **Read (Gjej)** | Menu opsioni 2 | `GetById(id)` | `GetById(string)` | cache |
| **Create (Shto)** | Menu opsioni 3 | `AddProduct()` + validim | `Add()` + `Save()` | shkruan CSV |
| **Update** | Menu opsioni 4 | `UpdateProduct()` + validim | `Update()` + `Save()` | shkruan CSV |
| **Delete** | Menu opsioni 5 | `DeleteProduct()` | `Delete()` + `Save()` | shkruan CSV |

## Struktura Finale

```
E-commerce/
├── backend/
│   ├── Models/
│   │   └── Product.cs          ← 6 atribute private, ToCsv/FromCsv
│   ├── Data/
│   │   ├── IRepository.cs      ← interface: GetAll/GetById/Add/Update/Delete/Save
│   │   ├── ProductFileRepository.cs  ← CSV read/write implementim
│   │   └── products.csv        ← 7 rekorde fillestare
│   ├── Services/
│   │   └── ProductService.cs   ← 5 metoda + validim
│   ├── UI/
│   │   └── ConsoleMenu.cs      ← menu me CRUD të plotë
│   ├── docs/
│   │   ├── architecture.md
│   │   ├── class-diagram.md
│   │   └── implementation.md   ← ky skedar
│   └── Program.cs              ← 10 rreshta
└── README.md
```
