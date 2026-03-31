using ECommerce.Models;
using ECommerce.Services;

namespace ECommerce.UI
{
    /// <summary>
    /// Console UI — lidh çdo operacion me Service → Repository → File.
    /// Rrjedha: UI → Service → Repository → CSV File
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
                switch (Console.ReadLine()?.Trim())
                {
                    case "1": MenuListProducts();  break;
                    case "2": MenuFindProduct();   break;
                    case "3": MenuAddProduct();    break;
                    case "4": MenuUpdateProduct(); break;
                    case "5": MenuDeleteProduct(); break;
                    case "6": MenuListStores();    break;
                    case "7": MenuCreateStore();   break;
                    case "8": MenuRegisterUser();  break;
                    case "9": MenuLoginUser();     break;
                    case "0": running = false;     break;
                    default:  Error("Opsion i pavlefshëm!"); break;
                }
            }
            Console.WriteLine("\n  Mirupafshim!\n");
        }

        // ── Metoda 1: Listo me filtrim ──────────────────────────────────────
        private void MenuListProducts()
        {
            Section("LISTA E PRODUKTEVE");
            Console.Write("  Filtro sipas kategorisë (Enter = të gjitha): ");
            var cat = Console.ReadLine()?.Trim() ?? "";

            var list = string.IsNullOrEmpty(cat)
                ? _productService.GetAll().ToList()
                : _productService.GetByCategory(cat).ToList();

            if (!list.Any()) { Info("Asnjë produkt nuk u gjet."); return; }

            Console.WriteLine($"\n  {"ID",-5} {"Emri",-25} {"Çmimi",10} {"Stoku",8} {"Kategoria",-15} {"StoreID"}");
            Console.WriteLine("  " + new string('─', 82));
            foreach (var p in list)
                Console.WriteLine($"  {p.Id,-5} {p.Name,-25} €{p.Price,9:F2} {p.Stock,8} {p.Category,-15} {p.StoreId}");
            Console.WriteLine($"\n  Gjithsej: {list.Count} produkte.");
        }

        // ── Metoda 2: Gjej sipas ID ─────────────────────────────────────────
        private void MenuFindProduct()
        {
            Section("GJEJ PRODUKT SIPAS ID");
            var input = Prompt("ID e produktit");
            if (!int.TryParse(input, out int id)) { Error("ID duhet të jetë numër i plotë."); return; }

            var p = _productService.GetById(id);
            if (p == null) { Error($"Produkti me ID {id} nuk u gjet."); return; }

            Console.WriteLine();
            Console.WriteLine($"  ID        : {p.Id}");
            Console.WriteLine($"  Emri      : {p.Name}");
            Console.WriteLine($"  Çmimi     : €{p.Price:F2}");
            Console.WriteLine($"  Stoku     : {p.Stock} copë");
            Console.WriteLine($"  Kategoria : {p.Category}");
            Console.WriteLine($"  Store ID  : {p.StoreId}");
            Console.WriteLine($"  Në stok   : {(p.IsInStock ? "Po ✓" : "Jo ✗")}");
        }

        // ── Metoda 3: Shto me validim ───────────────────────────────────────
        private void MenuAddProduct()
        {
            Section("SHTO PRODUKT TË RI");
            try
            {
                var name = Prompt("Emri i produktit");
                if (string.IsNullOrWhiteSpace(name))
                { Error("Emri nuk mund të jetë bosh!"); return; }

                var priceStr = Prompt("Çmimi (€)");
                if (!decimal.TryParse(priceStr, out decimal price) || price <= 0)
                { Error("Çmimi duhet të jetë numër pozitiv (> 0)!"); return; }

                var stockStr = Prompt("Stoku (copë)");
                if (!int.TryParse(stockStr, out int stock) || stock < 0)
                { Error("Stoku duhet të jetë numër jo-negativ!"); return; }

                var category = Prompt("Kategoria");
                if (string.IsNullOrWhiteSpace(category))
                { Error("Kategoria nuk mund të jetë bosh!"); return; }

                var storeId = Prompt("Store ID");
                if (string.IsNullOrWhiteSpace(storeId))
                { Error("Store ID nuk mund të jetë bosh!"); return; }

                var product = new Product(0, name, price, stock, category, storeId);
                _productService.AddProduct(product);
                Success($"Produkti '{name}' u shtua! (ID: {product.Id}) — u ruajt në products.csv");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // ── Update ──────────────────────────────────────────────────────────
        private void MenuUpdateProduct()
        {
            Section("PËRDITËSO PRODUKT (UPDATE)");
            try
            {
                var input = Prompt("ID e produktit");
                if (!int.TryParse(input, out int id)) { Error("ID duhet të jetë numër."); return; }

                var existing = _productService.GetById(id);
                if (existing == null) { Error($"Produkti me ID {id} nuk ekziston."); return; }

                Console.WriteLine($"\n  Produkti aktual: {existing}");
                Console.WriteLine("  (Shtyp Enter për të mbajtur vlerën aktuale)\n");

                var name = Prompt($"Emri i ri [{existing.Name}]");
                if (string.IsNullOrWhiteSpace(name)) name = existing.Name;

                var priceStr = Prompt($"Çmimi i ri [{existing.Price:F2}]");
                decimal price = string.IsNullOrWhiteSpace(priceStr)
                    ? existing.Price
                    : (decimal.TryParse(priceStr, out decimal p) && p > 0 ? p : existing.Price);

                var stockStr = Prompt($"Stoku i ri [{existing.Stock}]");
                int stock = string.IsNullOrWhiteSpace(stockStr)
                    ? existing.Stock
                    : (int.TryParse(stockStr, out int s) && s >= 0 ? s : existing.Stock);

                var category = Prompt($"Kategoria [{existing.Category}]");
                if (string.IsNullOrWhiteSpace(category)) category = existing.Category;

                var updated = new Product(existing.Id, name, price, stock, category, existing.StoreId);
                _productService.UpdateProduct(updated);
                Success($"Produkti ID:{id} u përditësua — u ruajt në products.csv");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // ── Delete ──────────────────────────────────────────────────────────
        private void MenuDeleteProduct()
        {
            Section("FSHI PRODUKT (DELETE)");
            try
            {
                var input = Prompt("ID e produktit");
                if (!int.TryParse(input, out int id)) { Error("ID duhet të jetë numër."); return; }

                var existing = _productService.GetById(id);
                if (existing == null) { Error($"Produkti me ID {id} nuk ekziston."); return; }

                Console.WriteLine($"\n  Do të fshihet: {existing}");
                Console.Write("  Je i sigurt? (po/jo): ");
                if (Console.ReadLine()?.Trim().ToLower() != "po")
                { Info("Operacioni u anulua."); return; }

                _productService.DeleteProduct(id);
                Success($"Produkti ID:{id} u fshi — u përditësua products.csv");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // ── Store ───────────────────────────────────────────────────────────
        private void MenuListStores()
        {
            Section("LISTA E DYQANEVE");
            var list = _storeService.GetAll().ToList();
            if (!list.Any()) { Info("Asnjë dyqan."); return; }
            foreach (var s in list) Console.WriteLine("  " + s);
        }

        private void MenuCreateStore()
        {
            Section("KRIJO DYQAN");
            try
            {
                var name    = Prompt("Emri i dyqanit");
                var ownerId = Prompt("Owner ID");
                if (string.IsNullOrWhiteSpace(name)) { Error("Emri nuk mund të jetë bosh!"); return; }
                var store = _storeService.CreateStore(name, ownerId);
                Success($"Dyqani u krijua! ID: {store.StoreId}");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // ── User ────────────────────────────────────────────────────────────
        private void MenuRegisterUser()
        {
            Section("REGJISTRIM");
            try
            {
                var name  = Prompt("Emri");
                var email = Prompt("Email");
                var pass  = Prompt("Fjalëkalimi");
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                { Error("Emri dhe email janë të detyrueshme!"); return; }
                var user = _userService.Register(name, email, pass);
                Success($"U regjistrua me sukses! ID: {user.UserId}");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void MenuLoginUser()
        {
            Section("HYRJE (LOGIN)");
            try
            {
                var email = Prompt("Email");
                var pass  = Prompt("Fjalëkalimi");
                var user  = _userService.Login(email, pass);
                if (user != null) Success($"Mirë se erdhe, {user.Name}! Roli: {user.Role}");
                else Error("Email ose fjalëkalim i gabuar.");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // ── Helpers ─────────────────────────────────────────────────────────
        private static void PrintMainMenu()
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║      E-COMMERCE PLATFORM — MENU KRYESORE    ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  ── PRODUKTET (CRUD) ──                     ║");
            Console.WriteLine("║  1. Listo produktet (me filtrim kategorisë)  ║");
            Console.WriteLine("║  2. Gjej produkt sipas ID                    ║");
            Console.WriteLine("║  3. Shto produkt të ri (Add)                 ║");
            Console.WriteLine("║  4. Përditëso produkt (Update)               ║");
            Console.WriteLine("║  5. Fshi produkt (Delete)                    ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  ── TJETËR ──                               ║");
            Console.WriteLine("║  6. Listo dyqanet                            ║");
            Console.WriteLine("║  7. Krijo dyqan                              ║");
            Console.WriteLine("║  8. Regjistrim                               ║");
            Console.WriteLine("║  9. Hyrje (Login)                            ║");
            Console.WriteLine("║  0. Dil                                      ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.Write("  Zgjidhni opsionin: ");
        }

        private static void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ╔══════════════════════════════════════════════════╗");
            Console.WriteLine("  ║   E-COMMERCE — CRUD System  |  .NET 8  v2.0     ║");
            Console.WriteLine("  ║   UI → Service → Repository → CSV File          ║");
            Console.WriteLine("  ╚══════════════════════════════════════════════════╝");
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
            Console.WriteLine($"\n  ╔─ {title} " + new string('─', Math.Max(0, 44 - title.Length)) + "╗");
            Console.ResetColor();
        }

        private static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ " + msg);
            Console.ResetColor();
        }

        private static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  ✗ " + msg);
            Console.ResetColor();
        }

        private static void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  » " + msg);
            Console.ResetColor();
        }
    }
}
