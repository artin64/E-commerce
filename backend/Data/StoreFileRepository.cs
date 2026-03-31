using ECommerce.Models;

namespace ECommerce.Data
{
    /// <summary>
    /// Concrete FileRepository for Store entities.
    /// Reads from and writes to stores.csv.
    /// </summary>
    public class StoreFileRepository : IRepository<Store>
    {
        private readonly string      _filePath;
        private          List<Store> _cache;

        public StoreFileRepository(string filePath = "Data/stores.csv")
        {
            _filePath = filePath;
            _cache    = new List<Store>();
            EnsureFileExists();
            LoadFromFile();
        }

        public IEnumerable<Store> GetAll() => _cache.AsReadOnly();

        public Store? GetById(string id) =>
            _cache.FirstOrDefault(s => s.StoreId == id);

        public void Add(Store entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _cache.Add(entity);
        }

        public void Update(Store entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            int idx = _cache.FindIndex(s => s.StoreId == entity.StoreId);
            if (idx == -1) throw new KeyNotFoundException($"Store ID {entity.StoreId} not found.");
            _cache[idx] = entity;
        }

        public void Delete(string id)
        {
            if (_cache.RemoveAll(s => s.StoreId == id) == 0)
                throw new KeyNotFoundException($"Store ID {id} not found.");
        }

        public void Save()
        {
            File.WriteAllLines(_filePath, _cache.Select(s => s.ToCsv()));
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
                         .Select(Store.FromCsv)
                         .ToList();
        }
    }
}
