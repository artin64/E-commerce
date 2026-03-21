using ECommerce.Models;
using ECommerce.Services;

namespace ECommerce.UI
{
    /// <summary>
    /// Console-based UI layer — handles all user interaction and display.
    ///
    /// SOLID:
    ///   • Single Responsibility — only presentation/input/output logic here.
    ///   • Dependency Inversion — depends on Service layer, never on Data layer directly.
    /// </summary>
    public class ConsoleMenu
    {
        // ── Private Attributes ──────────────────────────────────────────────
        private readonly ProductService _productService;
        private readonly StoreService   _storeService;
        private readonly OrderService   _orderService;
        private readonly UserService    _userService;

        // ── Constructor ─────────────────────────────────────────────────────
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

        // ── Public Methods ──────────────────────────────────────────────────

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
                    case "1": ShowAllProducts();  break;
                    case "2": ShowAllStores();    break;
                    case "3": AddProductMenu();   break;
                    case "4": CreateStoreMenu();  break;
                    case "5": PlaceOrderMenu();   break;
                    case "6": ShowAllOrders();    break;
                    case "7": RegisterMenu();     break;
                    case "8": LoginMenu();        break;
                    case "0": running = false;    break;
                    default:  Error("Invalid option."); break;
                }
            }
            Console.WriteLine("\n  Goodbye!\n");
        }

        // ── Private Menu Screens ────────────────────────────────────────────

        private void ShowAllProducts()
        {
            Section("ALL PRODUCTS");
            var list = _productService.GetAll().ToList();
            if (!list.Any()) { Info("No products found."); return; }
            foreach (var p in list) Console.WriteLine("  " + p);
        }

        private void ShowAllStores()
        {
            Section("ALL STORES");
            var list = _storeService.GetAll().ToList();
            if (!list.Any()) { Info("No stores found."); return; }
            foreach (var s in list) Console.WriteLine("  " + s);
        }

        private void ShowAllOrders()
        {
            Section("ALL ORDERS");
            var list = _orderService.GetAll().ToList();
            if (!list.Any()) { Info("No orders found."); return; }
            foreach (var o in list) Console.WriteLine("  " + o);
        }

        private void AddProductMenu()
        {
            Section("ADD PRODUCT");
            try
            {
                var name     = Prompt("Name");
                var price    = decimal.Parse(Prompt("Price"));
                var stock    = int.Parse(Prompt("Stock"));
                var category = Prompt("Category");
                var storeId  = Prompt("Store ID");
                _productService.AddProduct(new Product(0, name, price, stock, category, storeId));
                Success($"Product '{name}' added.");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void CreateStoreMenu()
        {
            Section("CREATE STORE");
            try
            {
                var name    = Prompt("Store Name");
                var ownerId = Prompt("Owner ID");
                var store   = _storeService.CreateStore(name, ownerId);
                Success($"Store created! ID: {store.StoreId}");
                Console.WriteLine($"  QR: {_storeService.GetQrCodeUrl(store.StoreId)}");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void PlaceOrderMenu()
        {
            Section("PLACE ORDER");
            try
            {
                var buyerId   = Prompt("Buyer ID");
                var storeId   = Prompt("Store ID");
                var productId = int.Parse(Prompt("Product ID"));
                var qty       = int.Parse(Prompt("Quantity"));
                var order     = _orderService.PlaceOrder(buyerId, storeId, productId, qty);
                Success("Order placed! " + order);
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void RegisterMenu()
        {
            Section("REGISTER USER");
            try
            {
                var name  = Prompt("Name");
                var email = Prompt("Email");
                var pass  = Prompt("Password");
                var user  = _userService.Register(name, email, pass);
                Success($"Registered! ID: {user.UserId}");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        private void LoginMenu()
        {
            Section("LOGIN");
            try
            {
                var email = Prompt("Email");
                var pass  = Prompt("Password");
                var user  = _userService.Login(email, pass);
                if (user != null) Success($"Welcome, {user.Name}! Role: {user.Role}");
                else Error("Invalid credentials.");
            }
            catch (Exception ex) { Error(ex.Message); }
        }

        // ── Private UI Helpers ──────────────────────────────────────────────

        private static void PrintMainMenu()
        {
            Console.WriteLine("\n╔════════════════════════════════════╗");
            Console.WriteLine("║       SHOPPLATFORM — MENU          ║");
            Console.WriteLine("╠════════════════════════════════════╣");
            Console.WriteLine("║  1. View All Products              ║");
            Console.WriteLine("║  2. View All Stores                ║");
            Console.WriteLine("║  3. Add New Product                ║");
            Console.WriteLine("║  4. Create New Store               ║");
            Console.WriteLine("║  5. Place Order                    ║");
            Console.WriteLine("║  6. View All Orders                ║");
            Console.WriteLine("║  7. Register                       ║");
            Console.WriteLine("║  8. Login                          ║");
            Console.WriteLine("║  0. Exit                           ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("  Choice: ");
        }

        private static void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ╔══════════════════════════════════════╗");
            Console.WriteLine("  ║   SHOPPLATFORM — Multi-Vendor v1.0  ║");
            Console.WriteLine("  ╚══════════════════════════════════════╝");
            Console.ResetColor();
        }

        private static string Prompt(string label)
        {
            Console.Write($"  {label}: ");
            return Console.ReadLine() ?? string.Empty;
        }

        private static void Section(string title) =>
            Console.WriteLine($"\n─── {title} " + new string('─', Math.Max(0, 38 - title.Length)));

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
