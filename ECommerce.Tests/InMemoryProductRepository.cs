using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Tests
{
    /// <summary>
    /// In-memory IRepository per teste — nuk perdor CSV fare.
    /// Siguron izolim te plote: testet nuk varen nga skedaret ne disk.
    /// </summary>
    public class InMemoryProductRepository : IRepository<Product>
    {
        private readonly List<Product> _store = new();

        public IEnumerable<Product> GetAll() => _store.AsReadOnly();

        public Product? GetById(string id) =>
            int.TryParse(id, out int intId)
                ? _store.FirstOrDefault(p => p.Id == intId)
                : null;

        public void Add(Product entity)
        {
            if (entity.Id == 0)
                entity.Id = _store.Count > 0 ? _store.Max(p => p.Id) + 1 : 1;
            _store.Add(entity);
        }

        public void Update(Product entity)
        {
            int idx = _store.FindIndex(p => p.Id == entity.Id);
            if (idx == -1)
                throw new KeyNotFoundException($"ID {entity.Id} nuk u gjet.");
            _store[idx] = entity;
        }

        public void Delete(string id)
        {
            if (!int.TryParse(id, out int intId))
                throw new ArgumentException("ID e pavlefshme.");
            if (_store.RemoveAll(p => p.Id == intId) == 0)
                throw new KeyNotFoundException($"ID {id} nuk u gjet.");
        }

        public void Save() { /* In-memory — asgjë për të ruajtur */ }
    }
}
