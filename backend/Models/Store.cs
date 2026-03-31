namespace ECommerce.Models
{
    /// <summary>
    /// Represents a vendor store on the platform.
    /// </summary>
    public class Store
    {
        // ── Private Attributes ──────────────────────────────────────────────
        private string _storeId;
        private string _name;
        private string _ownerId;
        private bool   _isVerified;
        private bool   _isActive;

        // ── Public Properties ───────────────────────────────────────────────
        public string StoreId    { get => _storeId;    set => _storeId    = value ?? throw new ArgumentNullException(nameof(value)); }
        public string Name       { get => _name;       set => _name       = value ?? throw new ArgumentNullException(nameof(value)); }
        public string OwnerId    { get => _ownerId;    set => _ownerId    = value ?? throw new ArgumentNullException(nameof(value)); }
        public bool   IsVerified { get => _isVerified; set => _isVerified = value; }
        public bool   IsActive   { get => _isActive;   set => _isActive   = value; }

        // ── Constructors ────────────────────────────────────────────────────
        public Store()
        {
            _storeId  = Guid.NewGuid().ToString("N")[..8].ToUpper();
            _name     = string.Empty;
            _ownerId  = string.Empty;
            _isActive = true;
        }

        public Store(string name, string ownerId)
        {
            _storeId  = Guid.NewGuid().ToString("N")[..8].ToUpper();
            _name     = name    ?? throw new ArgumentNullException(nameof(name));
            _ownerId  = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
            _isActive = true;
        }

        // ── Public Methods ──────────────────────────────────────────────────

        /// <summary>Serializes store to CSV row.</summary>
        public string ToCsv() =>
            $"{_storeId},{_name},{_ownerId},{_isVerified},{_isActive}";

        /// <summary>Deserializes CSV row to Store.</summary>
        public static Store FromCsv(string csvLine)
        {
            var p = csvLine.Split(',');
            return new Store
            {
                StoreId    = p[0],
                Name       = p[1],
                OwnerId    = p[2],
                IsVerified = bool.Parse(p[3]),
                IsActive   = bool.Parse(p[4])
            };
        }

        public override string ToString() =>
            $"[{_storeId}] {_name} | Owner:{_ownerId} | Verified:{_isVerified} | Active:{_isActive}";
    }
}
