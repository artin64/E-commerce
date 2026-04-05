using ECommerce.Models;
using ECommerce.Services;

namespace ECommerce.UI
{
    /// <summary>
    /// Console UI — Rrjedha: UI → Service → Repository → CSV
    /// Sprint 2: Shtuar Search(2), FilterByPrice(3), Statistics(4), error handling i plote.
    /// </summary>
    public class ConsoleMenu
    {
        private readonly ProductService _productService;
        private readonly StoreService   _storeService;
        private readonly OrderService   _orderService;
        private readonly UserService    _userService;

        public ConsoleMenu(
            ProductService productService,
            StoreService   storeService,
            OrderService   orderService,
            UserService    userService)
        {
            _productService = productService;
            _storeService   = storeService;
            _orderService   = orderService;
            _userService    = userService;
        }

        public void Run()
        {
            Console.Clear();
            PrintBanner();
            bool running = true;
            while (running)
            {
                PrintMainMenu();
                var choice = Console.ReadLine()?.Trim();
                switch (choice)
                {
                    case "1":  MenuListProducts();   break;
                    case "2":  MenuSearchProducts(); break;  // SPRINT 2
                    case "3":  MenuFilterByPrice();  break;  // SPRINT 2
                    case "4":  MenuStatistics();     break;  // SPRINT 2
                    case "5":  MenuFindProduct();    break;
                    case "6":  MenuAddProduct();     break;
                    case "7":  MenuUpdateProduct();  break;
                    case "8":  MenuDeleteProduct();  break;
                    case "9":  MenuListStores();     break;
                    case "10": MenuCreateStore();    break;
                    case "11": MenuRegisterUser();   break;
                    case "12": MenuLoginUser();      break;
                    case "0":  running = false;      break;
                    default:   Error("Opsion i pavlefshëm! Zgjidhni 0-12."); break;
                }
            }
            Console.WriteLine("\n  Mirupafshim!\n");
        }

        // ── 1: Listo ────────────────────────────────────────────────────────
        private void MenuListProducts()
        {
            Section("LISTA E PRODUKTEVE");
            try
            {
                Console.Write("  Filtro sipas kategorise (Enter = te gjitha): ");
                var cat = Console.ReadLine()?.Trim() ?? "";
                var list = string.IsNullOrEmpty(cat)
                    ? _productService.GetAll().ToList()
                    : _productService.GetByCategory(cat).ToList();
                if (!list.Any()) { Info("Asnje produkt nuk u gjet."); return; }
                PrintProductTable(list);
                Console.WriteLine($"\n  Gjithsej: {list.Count} produkte.");
            }
            catch (Exception ex) { Error($"Gabim: {ex.Message}"); }
        }

        // ── 2: SEARCH — FEATURE E RE Sprint 2 ──────────────────────────────
        private void MenuSearchProducts()
        {
            Section("KERKIM PRODUKTESH");
            try
            {
                var keyword = Prompt("Shkruaj emrin ose kategorine");

                // ERROR HANDLING: input bosh
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    Error("Ju lutem shkruani te pakten nje karakter per kerkim.");
                    return;
                }

                // UI → Service → Repository
                var results = _productService.Search(keyword).ToList();

                if (!results.Any())
                {
                    Info($"Asnje produkt nuk u gjet per '{keyword}'.");
                    return;
                }

                Info($"U gjetën {results.Count} rezultate per '{keyword}':");
                PrintProductTable(results);
            }
            catch (Exception ex) { Error($"Gabim gjate kërkimit: {ex.Message}"); }
        }

        // ── 3: FILTER BY PRICE — FEATURE E RE Sprint 2 ─────────────────────
        private void MenuFilterByPrice()
        {
            Section("FILTRIM SIPAS CMIMIT");
            try
            {
                var minStr = Prompt("Cmimi minimal (€) — p.sh. 10");

                // ERROR HANDLING: input jo-numerik per min
                if (!decimal.TryParse(minStr, out decimal min) || min < 0)
                {
                    Error("Ju lutem shkruani numer valid jo-negativ (p.sh. 0 ose 10.50).");
                    return;
                }

                var maxStr = Prompt("Cmimi maksimal (€) — p.sh. 500");

                // ERROR HANDLING: input jo-numerik per max
                if (!decimal.TryParse(maxStr, out decimal max) || max < 0)
                {
                    Error("Ju lutem shkruani numer valid jo-negativ (p.sh. 500).");
                    return;
                }

                // UI → Service → Repository
                var results = _productService.FilterByPrice(min, max).ToList();

                if (!results.Any())
                {
                    Info($"Asnje produkt ne rangun €{min:F2} — €{max:F2}.");
                    return;
                }

                Info($"Produktet me cmim €{min:F2} — €{max:F2}:");
                PrintProductTable(results);
            }
            catch (ArgumentException ex) { Error(ex.Message); }
            catch (Exception ex)         { Error($"Gabim: {ex.Message}"); }
        }

        // ── 4: STATISTICS — FEATURE E RE Sprint 2 ──────────────────────────
        private void MenuStatistics()
        {
            Section("STATISTIKAT E PRODUKTEVE");
            try
            {
                // UI → Service → Repository
                var stats = _productService.GetStatistics();

                if (stats.Count == 0)
                {
                    Info("Nuk ka produkte per te llogaritur statistika.");
                    return;
                }

                Console.WriteLine();
                Console.WriteLine($"  {"Numri i produkteve",-28} {stats.Count}");
                Console.WriteLine($"  {"Cmimi total",-28} €{stats.Total:F2}");
                Console.WriteLine($"  {"Cmimi mesatar",-28} €{stats.Average:F2}");
                Console.WriteLine($"  {"Cmimi maksimal",-28} €{stats.Max:F2}");
                Console.WriteLine($"  {"Cmimi minimal",-28} €{stats.Min:F2}");
            }
            catch (Exception ex) { Error($"Gabim gjate kalkulimit: {ex.Message}"); }
        }

        // ── 5: Gjej sipas ID ─────────────────────────────────────────────────
        private void MenuFindProduct()
        {
            Section("GJEJ PRODUKT SIPAS ID");
            try
            {
                var input = Prompt("ID e produktit");

                // ERROR HANDLING: input jo-numerik
                if (!int.TryParse(input, out int id))
                {
                    Error("Ju lutem shkruani numer valid per ID (p.sh. 1, 2, 3).");
                    return;
                }

                var p = _productService.GetById(id);

                // ERROR HANDLING: ID nuk ekziston
                if (p == null)
                {
                    Error($"Produkti me ID {id} nuk u gjet.");
                    return;
                }

                Console.WriteLine();
                Console.WriteLine($"  ID        : {p.Id}");
                Console.WriteLine($"  Emri      : {p.Name}");
                Console.WriteLine($"  Cmimi     : €{p.Price:F2}");
                Console.WriteLine($"  Stoku     : {p.Stock} cope");
                Console.WriteLine($"  Kategoria : {p.Category}");
                Console.WriteLine($"  Store ID  : {p.StoreId}");
                Console.WriteLine($"  Ne stok   : {(p.IsInStock ? "Po" : "Jo")}");
            }
            catch (Exception ex) { Error($"Gabim: {ex.Message}"); }
        }

        // ── 6: Shto ─────────────────────────────────────────────────────────
        private void MenuAddProduct()
        {
            Section("SHTO PRODUKT TE RI");
            try
            {
                var name = Prompt("Emri i produktit");
                if (string.IsNullOrWhiteSpace(name))
                { Error("Emri nuk mund te jete bosh!"); return; }

                var priceStr = Prompt("Cmimi (€) — p.sh. 29.99");
                // ERROR HANDLING: input jo-numerik per cmim
                if (!decimal.TryParse(priceStr, out decimal price) || price <= 0)
                { Error("Ju lutem shkruani numer valid pozitiv per cmimin (p.sh. 29.99)."); return; }

                var stockStr = Prompt("Stoku (cope) — p.sh. 50");
                // ERROR HANDLING: input jo-numerik per stok
                if (!int.TryParse(stockStr, out int stock) || stock < 0)
                { Error("Ju lutem shkruani numer valid jo-negativ per stokun (p.sh. 10)."); return; }

                var category = Prompt("Kategoria");
                if (string.IsNullOrWhiteSpace(category))
                { Error("Kategoria nuk mund te jete bosh!"); return; }

                var storeId = Prompt("Store ID");
                if (string.IsNullOrWhiteSpace(storeId))
                { Error("Store ID nuk mund te jete bosh!"); return; }

                var product = new Product(0, name, price, stock, category, storeId);
                _productService.AddProduct(product);
                Success($"Produkti '{name}' u shtua! (ID: {product.Id}) — u ruajt ne products.csv");
            }
            catch (ArgumentException ex) { Error(ex.Message); }
            catch (Exception ex)         { Error($"Gabim i papritur: {ex.Message}"); }
        }

        // ── 7: Update ───────────────────────────────────────────────────────
        private void MenuUpdateProduct()
        {
            Section("PERDITESO PRODUKT (UPDATE)");
            try
            {
                var input = Prompt("ID e produktit");
                if (!int.TryParse(input, out int id))
                { Error("Ju lutem shkruani numer valid per ID."); return; }

                var existing = _productService.GetById(id);
                if (existing == null)
                { Error($"Produkti me ID {id} nuk ekziston."); return; }

                Console.WriteLine($"\n  Produkti aktual: {existing}");
                Console.WriteLine("  (Shtyp Enter per te mbajtur vleren aktuale)\n");

                var name = Prompt($"Emri i ri [{existing.Name}]");
                if (string.IsNullOrWhiteSpace(name)) name = existing.Name;

                var priceStr = Prompt($"Cmimi i ri [{existing.Price:F2}]");
                decimal price = existing.Price;
                if (!string.IsNullOrWhiteSpace(priceStr))
                {
                    if (!decimal.TryParse(priceStr, out decimal parsed) || parsed <= 0)
                    { Error("Ju lutem shkruani numer valid pozitiv per cmimin."); return; }
                    price = parsed;
                }

                var stockStr = Prompt($"Stoku i ri [{existing.Stock}]");
                int stock = existing.Stock;
                if (!string.IsNullOrWhiteSpace(stockStr))
                {
                    if (!int.TryParse(stockStr, out int parsed) || parsed < 0)
                    { Error("Ju lutem shkruani numer valid jo-negativ per stokun."); return; }
                    stock = parsed;
                }

                var category = Prompt($"Kategoria [{existing.Category}]");
                if (string.IsNullOrWhiteSpace(category)) category = existing.Category;

                var updated = new Product(existing.Id, name, price, stock, category, existing.StoreId);
                _productService.UpdateProduct(updated);
                Success($"Produkti ID:{id} u perditesua — u ruajt ne products.csv");
            }
            catch (ArgumentException ex)    { Error(ex.Message); }
            catch (KeyNotFoundException ex) { Error(ex.Message); }
            catch (Exception ex)            { Error($"Gabim i papritur: {ex.Message}"); }
        }

        // ── 8: Delete ───────────────────────────────────────────────────────
        private void MenuDeleteProduct()
        {
            Section("FSHI PRODUKT (DELETE)");
            try
            {
                var input = Prompt("ID e produktit");
                if (!int.TryParse(input, out int id))
                { Error("Ju lutem shkruani numer valid per ID."); return; }

                var existing = _productService.GetById(id);
                if (existing == null)
                { Error($"Produkti me ID {id} nuk ekziston."); return; }

                Console.WriteLine($"\n  Do te fshihet: {existing}");
                Console.Write("  Je i sigurt? (po/jo): ");
                var confirm = Console.ReadLine()?.Trim().ToLower();
                if (confirm != "po") { Info("Operacioni u anulua."); return; }

                _productService.DeleteProduct(id);
                Success($"Produkti ID:{id} u fshi — u perditesua products.csv");
            }
            catch (KeyNotFoundException ex) { Error(ex.Message); }
            catch (Exception ex)            { Error($"Gabim i papritur: {ex.Message}"); }
        }

        // ── 9-12 ────────────────────────────────────────────────────────────
        private void MenuListStores()
        {
            Section("LISTA E DYQANEVE");
            try
            {
                var list = _storeService.GetAll().ToList();
                if (!list.Any()) { Info("Asnje dyqan."); return; }
                foreach (var s in list) Console.WriteLine("  " + s);
            }
            catch (Exception ex) { Error($"Gabim: {ex.Message}"); }
        }

        private void MenuCreateStore()
        {
            Section("KRIJO DYQAN");
            try
            {
                var name    = Prompt("Emri i dyqanit");
                var ownerId = Prompt("Owner ID");
                if (string.IsNullOrWhiteSpace(name))    { Error("Emri nuk mund te jete bosh!"); return; }
                if (string.IsNullOrWhiteSpace(ownerId)) { Error("Owner ID nuk mund te jete bosh!"); return; }
                var store = _storeService.CreateStore(name, ownerId);
                Success($"Dyqani u krijua! ID: {store.StoreId}");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void MenuRegisterUser()
        {
            Section("REGJISTRIM");
            try
            {
                var name  = Prompt("Emri");
                var email = Prompt("Email");
                var pass  = Prompt("Fjalekalimi");
                if (string.IsNullOrWhiteSpace(name))  { Error("Emri nuk mund te jete bosh!"); return; }
                if (string.IsNullOrWhiteSpace(email)) { Error("Email nuk mund te jete bosh!"); return; }
                if (string.IsNullOrWhiteSpace(pass))  { Error("Fjalekalimi nuk mund te jete bosh!"); return; }
                var user = _userService.Register(name, email, pass);
                Success($"U regjistrua! ID: {user.UserId}");
            }
            catch (InvalidOperationException ex) { Error(ex.Message); }
            catch (Exception ex) { Error($"Gabim: {ex.Message}"); }
        }

        private void MenuLoginUser()
        {
            Section("HYRJE (LOGIN)");
            try
            {
                var email = Prompt("Email");
                var pass  = Prompt("Fjalekalimi");
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
                { Error("Email dhe fjalekalimi jane te detyrueshme!"); return; }
                var user = _userService.Login(email, pass);
                if (user != null) Success($"Mire se erdhe, {user.Name}! Roli: {user.Role}");
                else Error("Email ose fjalekalim i gabuar.");
            }
            catch (Exception ex) { Error($"Gabim: {ex.Message}"); }
        }

        // ── Helpers ─────────────────────────────────────────────────────────
        private static void PrintProductTable(List<Product> list)
        {
            Console.WriteLine($"\n  {"ID",-5} {"Emri",-25} {"Cmimi",10} {"Stoku",8} {"Kategoria",-15} {"StoreID"}");
            Console.WriteLine("  " + new string('-', 80));
            foreach (var p in list)
                Console.WriteLine($"  {p.Id,-5} {p.Name,-25} €{p.Price,9:F2} {p.Stock,8} {p.Category,-15} {p.StoreId}");
        }

        private static void PrintMainMenu()
        {
            Console.WriteLine("\n+==================================================+");
            Console.WriteLine("|   E-COMMERCE PLATFORM  v2.0  |  Sprint 2        |");
            Console.WriteLine("+==================================================+");
            Console.WriteLine("|  -- KERKIM & STATISTIKA (Sprint 2) --           |");
            Console.WriteLine("|  1.  Listo te gjitha produktet                   |");
            Console.WriteLine("|  2.  Kerko produkte sipas emrit/kategorise       |");
            Console.WriteLine("|  3.  Filtro sipas cmimit (min - max)             |");
            Console.WriteLine("|  4.  Statistika (total, mesatare, max, min)      |");
            Console.WriteLine("+--------------------------------------------------+");
            Console.WriteLine("|  -- CRUD PRODUKTEVE --                          |");
            Console.WriteLine("|  5.  Gjej produkt sipas ID                       |");
            Console.WriteLine("|  6.  Shto produkt te ri (Add)                    |");
            Console.WriteLine("|  7.  Perditeso produkt (Update)                  |");
            Console.WriteLine("|  8.  Fshi produkt (Delete)                       |");
            Console.WriteLine("+--------------------------------------------------+");
            Console.WriteLine("|  -- TJETER --                                   |");
            Console.WriteLine("|  9.  Listo dyqanet                               |");
            Console.WriteLine("|  10. Krijo dyqan                                 |");
            Console.WriteLine("|  11. Regjistrim                                  |");
            Console.WriteLine("|  12. Hyrje (Login)                               |");
            Console.WriteLine("|  0.  Dil                                         |");
            Console.WriteLine("+==================================================+");
            Console.Write("  Zgjidhni opsionin: ");
        }

        private static void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  +================================================+");
            Console.WriteLine("  |  E-COMMERCE  |  Sprint 2  |  .NET 8  v2.0      |");
            Console.WriteLine("  |  UI -> Service -> Repository -> CSV             |");
            Console.WriteLine("  +================================================+");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static string Prompt(string label)
        {
            Console.Write($"  {label}: ");
            return Console.ReadLine() ?? string.Empty;
        }

        private static void Section(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n  +-- {title} " + new string('-', Math.Max(0, 42 - title.Length)) + "+");
            Console.ResetColor();
        }

        private static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  [OK] " + msg);
            Console.ResetColor();
        }

        private static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  [ERROR] " + msg);
            Console.ResetColor();
        }

        private static void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  [INFO] " + msg);
            Console.ResetColor();
        }
    }
}
