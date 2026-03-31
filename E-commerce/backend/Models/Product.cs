namespace ECommerce.Models
{
    /// <summary>
    /// Represents a product in a vendor store.
    /// </summary>
    public class Product
    {
        // ── Private Attributes ──────────────────────────────────────────────
        private int     _id;
        private string  _name;
        private decimal _price;
        private int     _stock;
        private string  _category;
        private string  _storeId;

        // ── Public Properties ───────────────────────────────────────────────
        public int     Id       { get => _id;       set => _id       = value; }
        public string  Name     { get => _name;      set => _name     = value ?? throw new ArgumentNullException(nameof(value)); }
        public decimal Price    { get => _price;     set => _price    = value >= 0 ? value : throw new ArgumentException("Price cannot be negative."); }
        public int     Stock    { get => _stock;     set => _stock    = value >= 0 ? value : throw new ArgumentException("Stock cannot be negative."); }
        public string  Category { get => _category;  set => _category = value ?? string.Empty; }
        public string  StoreId  { get => _storeId;   set => _storeId  = value ?? throw new ArgumentNullException(nameof(value)); }

        // ── Computed Property ───────────────────────────────────────────────
        public bool IsInStock => _stock > 0;

        // ── Constructors ────────────────────────────────────────────────────
        public Product()
        {
            _name     = string.Empty;
            _category = string.Empty;
            _storeId  = string.Empty;
        }

        public Product(int id, string name, decimal price, int stock, string category, string storeId)
        {
            _id       = id;
            _name     = name     ?? throw new ArgumentNullException(nameof(name));
            _price    = price    >= 0 ? price  : throw new ArgumentException("Price cannot be negative.");
            _stock    = stock    >= 0 ? stock  : throw new ArgumentException("Stock cannot be negative.");
            _category = category ?? string.Empty;
            _storeId  = storeId  ?? throw new ArgumentNullException(nameof(storeId));
        }

        // ── Public Methods ──────────────────────────────────────────────────

        /// <summary>Serializes product to CSV row.</summary>
        public string ToCsv() =>
            $"{_id},{EscapeCsv(_name)},{_price},{_stock},{EscapeCsv(_category)},{_storeId}";

        /// <summary>Deserializes CSV row to Product.</summary>
        public static Product FromCsv(string csvLine)
        {
            var p = csvLine.Split(',');
            return new Product(
                int.Parse(p[0]),
                p[1], decimal.Parse(p[2]),
                int.Parse(p[3]), p[4], p[5]);
        }

        public override string ToString() =>
            $"[{_id}] {_name} | €{_price:F2} | Stock:{_stock} | {_category} | Store:{_storeId}";

        // ── Private Helpers ─────────────────────────────────────────────────
        private static string EscapeCsv(string value) =>
            value.Contains(',') ? $"\"{value}\"" : value;
    }
}
