using ECommerce.Models;

namespace ECommerce.Data
{
    /// <summary>
    /// Concrete FileRepository for User entities.
    /// Reads from and writes to users.csv.
    /// </summary>
    public class UserFileRepository : IRepository<User>
    {
        private readonly string     _filePath;
        private          List<User> _cache;

        public UserFileRepository(string filePath = "Data/users.csv")
        {
            _filePath = filePath;
            _cache    = new List<User>();
            EnsureFileExists();
            LoadFromFile();
        }

        public IEnumerable<User> GetAll() => _cache.AsReadOnly();

        public User? GetById(string id) =>
            _cache.FirstOrDefault(u => u.UserId == id);

        public void Add(User entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _cache.Add(entity);
        }

        public void Update(User entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            int idx = _cache.FindIndex(u => u.UserId == entity.UserId);
            if (idx == -1) throw new KeyNotFoundException($"User ID {entity.UserId} not found.");
            _cache[idx] = entity;
        }

        public void Delete(string id)
        {
            if (_cache.RemoveAll(u => u.UserId == id) == 0)
                throw new KeyNotFoundException($"User ID {id} not found.");
        }

        public void Save()
        {
            File.WriteAllLines(_filePath, _cache.Select(u => u.ToCsv()));
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
                         .Select(User.FromCsv)
                         .ToList();
        }
    }
}
