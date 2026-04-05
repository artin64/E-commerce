using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Services
{
    /// <summary>
    /// Business logic layer per Produkte.
    /// SOLID:
    ///   S — nje pergjegjesi: vetem rregullat e biznesit per produkte
    ///   D — varet nga IRepository abstrakt, jo nga FileRepository konkret
    /// </summary>
    public class ProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IRepository<Product> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // ── Lexim ───────────────────────────────────────────────────────────

        public IEnumerable<Product> GetAll()        => _repository.GetAll();
        public Product?             GetById(int id) => _repository.GetById(id.ToString());

        public IEnumerable<Product> GetByStore(string storeId) =>
            _repository.GetAll().Where(p => p.StoreId == storeId);

        public IEnumerable<Product> GetByCategory(string category) =>
            _repository.GetAll().Where(p =>
                p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

        // ── FEATURE E RE Sprint 2: Search ───────────────────────────────────

        /// <summary>
        /// Kerkon produkte sipas emrit ose kategorise (case-insensitive).
        /// Rrjedha: UI → Search(keyword) → GetAll() → filter LINQ → UI
        /// </summary>
        public IEnumerable<Product> Search(string keyword) =>
            _repository.GetAll().Where(p =>
                p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase));

        // ── FEATURE E RE Sprint 2: FilterByPrice ────────────────────────────

        /// <summary>
        /// Filtron produkte sipas rangut te cmimit (min-max).
        /// Logjika e validimit eshte ne Service, jo ne UI dhe jo ne Repository.
        /// Rrjedha: UI → FilterByPrice(min,max) → GetAll() → filter LINQ → UI
        /// </summary>
        public IEnumerable<Product> FilterByPrice(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0)
                throw new ArgumentException("Cmimi minimal nuk mund te jete negativ.");
            if (maxPrice < minPrice)
                throw new ArgumentException("Cmimi maksimal duhet te jete >= cmimit minimal.");

            return _repository.GetAll()
                               .Where(p => p.Price >= minPrice && p.Price <= maxPrice);
        }

        // ── FEATURE E RE Sprint 2: GetStatistics ────────────────────────────

        /// <summary>
        /// Llogarit statistikat: numri, totali, mesatarja, max, min.
        /// E gjithe logjika e kalkulimit eshte ne Service layer.
        /// Rrjedha: UI → GetStatistics() → GetAll() → kalkulim LINQ → UI
        /// </summary>
        public ProductStatistics GetStatistics()
        {
            var all = _repository.GetAll().ToList();

            if (!all.Any())
                return new ProductStatistics(Count: 0, Total: 0, Average: 0, Max: 0, Min: 0);

            return new ProductStatistics(
                Count:   all.Count,
                Total:   all.Sum(p => p.Price),
                Average: Math.Round(all.Average(p => p.Price), 2),
                Max:     all.Max(p => p.Price),
                Min:     all.Min(p => p.Price)
            );
        }

        // ── CRUD ────────────────────────────────────────────────────────────

        public void AddProduct(Product product)
        {
            Validate(product);
            _repository.Add(product);
            _repository.Save();
        }

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

        public bool ReduceStock(int productId, int qty)
        {
            var product = _repository.GetById(productId.ToString());
            if (product == null || product.Stock < qty) return false;
            product.Stock -= qty;
            _repository.Update(product);
            _repository.Save();
            return true;
        }

        // ── Validim privat ──────────────────────────────────────────────────

        private static void Validate(Product p)
        {
            if (string.IsNullOrWhiteSpace(p.Name))
                throw new ArgumentException("Emri nuk mund te jete bosh!");
            if (string.IsNullOrWhiteSpace(p.StoreId))
                throw new ArgumentException("StoreId nuk mund te jete bosh!");
            if (p.Price <= 0)
                throw new ArgumentException("Cmimi duhet te jete > 0!");
            if (p.Stock < 0)
                throw new ArgumentException("Stoku nuk mund te jete negativ!");
        }
    }
}
