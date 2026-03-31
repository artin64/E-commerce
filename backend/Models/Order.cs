namespace ECommerce.Models
{
    public enum OrderStatus { Pending, Confirmed, Shipped, Delivered, Cancelled }

    /// <summary>
    /// Represents a purchase order placed by a buyer.
    /// </summary>
    public class Order
    {
        // ── Private Attributes ──────────────────────────────────────────────
        private int         _orderId;
        private string      _buyerId;
        private string      _storeId;
        private int         _productId;
        private int         _quantity;
        private decimal     _totalPrice;
        private OrderStatus _status;
        private DateTime    _createdAt;

        // ── Public Properties ───────────────────────────────────────────────
        public int         OrderId    { get => _orderId;    set => _orderId    = value; }
        public string      BuyerId    { get => _buyerId;    set => _buyerId    = value ?? throw new ArgumentNullException(nameof(value)); }
        public string      StoreId    { get => _storeId;    set => _storeId    = value ?? throw new ArgumentNullException(nameof(value)); }
        public int         ProductId  { get => _productId;  set => _productId  = value; }
        public int         Quantity   { get => _quantity;   set => _quantity   = value > 0 ? value : throw new ArgumentException("Quantity must be > 0."); }
        public decimal     TotalPrice { get => _totalPrice; set => _totalPrice = value >= 0 ? value : throw new ArgumentException("Total cannot be negative."); }
        public OrderStatus Status     { get => _status;     set => _status     = value; }
        public DateTime    CreatedAt  { get => _createdAt;  set => _createdAt  = value; }

        // ── Constructors ────────────────────────────────────────────────────
        public Order()
        {
            _buyerId   = string.Empty;
            _storeId   = string.Empty;
            _status    = OrderStatus.Pending;
            _createdAt = DateTime.UtcNow;
        }

        public Order(int orderId, string buyerId, string storeId, int productId, int quantity, decimal totalPrice)
        {
            _orderId    = orderId;
            _buyerId    = buyerId    ?? throw new ArgumentNullException(nameof(buyerId));
            _storeId    = storeId    ?? throw new ArgumentNullException(nameof(storeId));
            _productId  = productId;
            _quantity   = quantity   > 0 ? quantity   : throw new ArgumentException("Quantity must be > 0.");
            _totalPrice = totalPrice >= 0 ? totalPrice : throw new ArgumentException("Total cannot be negative.");
            _status     = OrderStatus.Pending;
            _createdAt  = DateTime.UtcNow;
        }

        // ── Public Methods ──────────────────────────────────────────────────

        /// <summary>Transitions order to a new status.</summary>
        public void UpdateStatus(OrderStatus newStatus) => _status = newStatus;

        /// <summary>Serializes order to CSV row.</summary>
        public string ToCsv() =>
            $"{_orderId},{_buyerId},{_storeId},{_productId},{_quantity},{_totalPrice},{_status},{_createdAt:O}";

        /// <summary>Deserializes CSV row to Order.</summary>
        public static Order FromCsv(string csvLine)
        {
            var p = csvLine.Split(',');
            return new Order
            {
                OrderId    = int.Parse(p[0]),
                BuyerId    = p[1], StoreId  = p[2],
                ProductId  = int.Parse(p[3]),
                Quantity   = int.Parse(p[4]),
                TotalPrice = decimal.Parse(p[5]),
                Status     = Enum.Parse<OrderStatus>(p[6]),
                CreatedAt  = DateTime.Parse(p[7])
            };
        }

        public override string ToString() =>
            $"Order#{_orderId} | Buyer:{_buyerId} | Prod:{_productId} x{_quantity} | €{_totalPrice:F2} | {_status}";
    }
}
