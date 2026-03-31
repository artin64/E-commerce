using ECommerce.Models;

namespace ECommerce.Data
{
    /// <summary>
    /// Concrete FileRepository for Product entities.
    /// Reads from and writes to products.csv.
    ///
    /// SOLID: Open/Closed Principle — this class is closed for modification.
    /// To add a new storage backend, create a new class implementing IRepository&lt;Product&gt;.
    /// </summary>
    public class ProductFileRepository : IRepository<Product>
    {
        // ── Private Attributes ──────────────────────────────────────────────
        private readonly string   _filePath;
        private          List<Product> _cache;

        // ── Constructor ─────────────────────────────────────────────────────
        public ProductFileRepository(string filePath = "Data/products.csv")
        {
            _filePath = filePath;
            _cache    = new List<Product>();
            EnsureFileExists();
            LoadFromFile();
        }

        // ── IRepository<Product> Implementation ────────────────────────────

        public IEnumerable<Product> GetAll() =>
            _cache.AsReadOnly();

        public Product? GetById(string id)
        {
            if (!int.TryParse(id, out int intId)) return null;
            return _cache.FirstOrDefault(p => p.Id == intId);
        }

        public void Add(Product entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            if (entity.Id == 0)
                entity.Id = _cache.Count > 0 ? _cache.Max(p => p.Id) + 1 : 1;
            _cache.Add(entity);
        }

        public void Update(Product entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            int idx = _cache.FindIndex(p => p.Id == entity.Id);
            if (idx == -1) throw new KeyNotFoundException($"Product ID {entity.Id} not found.");
            _cache[idx] = entity;
        }

        public void Delete(string id)
        {
            if (!int.TryParse(id, out int intId))
                throw new ArgumentException($"Invalid ID: {id}");
            if (_cache.RemoveAll(p => p.Id == intId) == 0)
                throw new KeyNotFoundException($"Product ID {id} not found.");
        }

        public void Save()
        {
            File.WriteAllLines(_filePath, _cache.Select(p => p.ToCsv()));
        }

        // ── Private Helpers ─────────────────────────────────────────────────

        private void EnsureFileExists()
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(_filePath)) File.WriteAllText(_filePath, string.Empty);
        }

        private void LoadFromFile()
        {
            _cache = File.ReadAllLines(_filePath)
                         .Where(l => !string.IsNullOrWhiteSpace(l))
                         .Select(Product.FromCsv)
                         .ToList();
        }
    }
}
