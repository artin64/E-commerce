using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Services
{
    /// <summary>
    /// Business logic layer for Product operations.
    ///
    /// SOLID:
    ///   • Single Responsibility — only product business rules live here.
    ///   • Dependency Inversion — depends on IRepository&lt;Product&gt; abstraction.
    /// </summary>
    public class ProductService
    {
        // ── Private Attributes ──────────────────────────────────────────────
        private readonly IRepository<Product> _repository;

        // ── Constructor (Dependency Injection) ──────────────────────────────
        public ProductService(IRepository<Product> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // ── Public Methods ──────────────────────────────────────────────────

        public IEnumerable<Product> GetAll()              => _repository.GetAll();
        public Product?             GetById(int id)       => _repository.GetById(id.ToString());

        public IEnumerable<Product> GetByStore(string storeId) =>
            _repository.GetAll().Where(p => p.StoreId == storeId);

        public IEnumerable<Product> GetByCategory(string category) =>
            _repository.GetAll().Where(p =>
                p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<Product> Search(string keyword) =>
            _repository.GetAll().Where(p =>
                p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase));

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

        /// <summary>Reduces stock after a successful order. Returns false if insufficient.</summary>
        public bool ReduceStock(int productId, int qty)
        {
            var product = _repository.GetById(productId.ToString());
            if (product == null || product.Stock < qty) return false;
            product.Stock -= qty;
            _repository.Update(product);
            _repository.Save();
            return true;
        }

        // ── Private Helpers ─────────────────────────────────────────────────

        private static void Validate(Product p)
        {
            if (string.IsNullOrWhiteSpace(p.Name))    throw new ArgumentException("Name required.");
            if (string.IsNullOrWhiteSpace(p.StoreId)) throw new ArgumentException("StoreId required.");
            if (p.Price < 0)                           throw new ArgumentException("Price cannot be negative.");
            if (p.Stock < 0)                           throw new ArgumentException("Stock cannot be negative.");
        }
    }
}
