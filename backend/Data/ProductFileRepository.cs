using ECommerce.Models;

namespace ECommerce.Data
{
    /// <summary>
    /// FileRepository per Product — lexon/ruan ne products.csv.
    /// ERROR HANDLING i plote Sprint 2: te gjitha operacionet e file jane brenda try-catch.
    /// </summary>
    public class ProductFileRepository : IRepository<Product>
    {
        private readonly string        _filePath;
        private          List<Product> _cache;

        public ProductFileRepository(string filePath = "Data/products.csv")
        {
            _filePath = filePath;
            _cache    = new List<Product>();
            EnsureFileExists();
            LoadFromFile();
        }

        public IEnumerable<Product> GetAll()    => _cache.AsReadOnly();

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
            if (idx == -1)
                throw new KeyNotFoundException($"Produkti me ID {entity.Id} nuk u gjet.");
            _cache[idx] = entity;
        }

        public void Delete(string id)
        {
            if (!int.TryParse(id, out int intId))
                throw new ArgumentException($"ID e pavlefshme: {id}");
            if (_cache.RemoveAll(p => p.Id == intId) == 0)
                throw new KeyNotFoundException($"Produkti me ID {id} nuk u gjet.");
        }

        /// <summary>
        /// ERROR HANDLING: catch IOException — shfaq mesazh, programi nuk crashon.
        /// </summary>
        public void Save()
        {
            try
            {
                EnsureFileExists();
                File.WriteAllLines(_filePath, _cache.Select(p => p.ToCsv()));
            }
            catch (IOException ex)
            {
                Console.WriteLine($"  [ERROR] Gabim gjate ruajtjes ne file: {ex.Message}");
            }
        }

        // ── Private Helpers ─────────────────────────────────────────────────

        private void EnsureFileExists()
        {
            try
            {
                var dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);
                if (!File.Exists(_filePath))
                    File.WriteAllText(_filePath, string.Empty);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"  [ERROR] Nuk u krijua skedari: {ex.Message}");
            }
        }

        /// <summary>
        /// ERROR HANDLING:
        ///   FileNotFoundException → "Skedari nuk u gjet. Po krijoj skedar te ri..."
        ///   IOException           → mesazh gabimi, vazhdon me liste boshe
        ///   Rreshti CSV i deformuar → [WARN] anashkalohet, programi nuk crashon
        /// </summary>
        private void LoadFromFile()
        {
            try
            {
                _cache = File.ReadAllLines(_filePath)
                             .Where(l => !string.IsNullOrWhiteSpace(l))
                             .Select(line =>
                             {
                                 try
                                 {
                                     return Product.FromCsv(line);
                                 }
                                 catch
                                 {
                                     Console.WriteLine($"  [WARN] Rreshti CSV i deformuar u anashkalua: {line}");
                                     return null!;
                                 }
                             })
                             .Where(p => p != null)
                             .ToList();
            }
            catch (FileNotFoundException)
            {
                // ERROR HANDLING — kerkesa specifike e detyres
                Console.WriteLine("  [INFO] Skedari products.csv nuk u gjet. Po krijoj skedar te ri...");
                _cache = new List<Product>();
                EnsureFileExists();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"  [ERROR] Gabim gjate leximit te skedarit: {ex.Message}");
                _cache = new List<Product>();
            }
        }
    }
}
