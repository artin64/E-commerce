using ECommerce.Models;

namespace ECommerce.Data
{
    /// <summary>
    /// Concrete FileRepository for Order entities.
    /// Reads from and writes to orders.csv.
    /// </summary>
    public class OrderFileRepository : IRepository<Order>
    {
        private readonly string      _filePath;
        private          List<Order> _cache;

        public OrderFileRepository(string filePath = "Data/orders.csv")
        {
            _filePath = filePath;
            _cache    = new List<Order>();
            EnsureFileExists();
            LoadFromFile();
        }

        public IEnumerable<Order> GetAll() => _cache.AsReadOnly();

        public Order? GetById(string id)
        {
            if (!int.TryParse(id, out int intId)) return null;
            return _cache.FirstOrDefault(o => o.OrderId == intId);
        }

        public void Add(Order entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            if (entity.OrderId == 0)
                entity.OrderId = _cache.Count > 0 ? _cache.Max(o => o.OrderId) + 1 : 1;
            _cache.Add(entity);
        }

        public void Update(Order entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            int idx = _cache.FindIndex(o => o.OrderId == entity.OrderId);
            if (idx == -1) throw new KeyNotFoundException($"Order ID {entity.OrderId} not found.");
            _cache[idx] = entity;
        }

        public void Delete(string id)
        {
            if (!int.TryParse(id, out int intId))
                throw new ArgumentException($"Invalid ID: {id}");
            if (_cache.RemoveAll(o => o.OrderId == intId) == 0)
                throw new KeyNotFoundException($"Order ID {id} not found.");
        }

        public void Save()
        {
            File.WriteAllLines(_filePath, _cache.Select(o => o.ToCsv()));
        }

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
                         .Select(Order.FromCsv)
                         .ToList();
        }
    }
}
