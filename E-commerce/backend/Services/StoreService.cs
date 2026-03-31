using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Services
{
    /// <summary>
    /// Business logic layer for Store / multi-vendor operations.
    ///
    /// SOLID:
    ///   • Single Responsibility — only store management rules live here.
    ///   • Dependency Inversion — depends on IRepository&lt;Store&gt;.
    /// </summary>
    public class StoreService
    {
        // ── Private Attributes ──────────────────────────────────────────────
        private readonly IRepository<Store> _repository;

        // ── Constructor ─────────────────────────────────────────────────────
        public StoreService(IRepository<Store> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // ── Public Methods ──────────────────────────────────────────────────

        public IEnumerable<Store> GetAll()             => _repository.GetAll();
        public Store?             GetById(string id)   => _repository.GetById(id);
        public IEnumerable<Store> GetActive()          => _repository.GetAll().Where(s => s.IsActive);

        public Store CreateStore(string name, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(name))    throw new ArgumentException("Store name required.");
            if (string.IsNullOrWhiteSpace(ownerId)) throw new ArgumentException("OwnerId required.");
            var store = new Store(name, ownerId);
            _repository.Add(store);
            _repository.Save();
            return store;
        }

        public void UpdateStore(Store store)
        {
            ArgumentNullException.ThrowIfNull(store);
            _repository.Update(store);
            _repository.Save();
        }

        public void VerifyStore(string storeId)
        {
            var store = _repository.GetById(storeId)
                ?? throw new KeyNotFoundException($"Store {storeId} not found.");
            store.IsVerified = true;
            _repository.Update(store);
            _repository.Save();
        }

        public void DeactivateStore(string storeId)
        {
            var store = _repository.GetById(storeId)
                ?? throw new KeyNotFoundException($"Store {storeId} not found.");
            store.IsActive = false;
            _repository.Update(store);
            _repository.Save();
        }

        /// <summary>Returns QR code URL for the store's public page.</summary>
        public string GetQrCodeUrl(string storeId) =>
            $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data=https://shopplatform.com/store/{storeId}";
    }
}
