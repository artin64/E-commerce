using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Services
{
    /// <summary>
    /// Business logic layer for Order operations.
    ///
    /// SOLID:
    ///   • Single Responsibility — only order rules live here.
    ///   • Dependency Inversion — depends on two IRepository abstractions.
    /// </summary>
    public class OrderService
    {
        // ── Private Attributes ──────────────────────────────────────────────
        private readonly IRepository<Order>   _orderRepo;
        private readonly IRepository<Product> _productRepo;

        // ── Constructor ─────────────────────────────────────────────────────
        public OrderService(IRepository<Order> orderRepo, IRepository<Product> productRepo)
        {
            _orderRepo   = orderRepo   ?? throw new ArgumentNullException(nameof(orderRepo));
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
        }

        // ── Public Methods ──────────────────────────────────────────────────

        public IEnumerable<Order> GetAll()                       => _orderRepo.GetAll();
        public Order?             GetById(int id)                => _orderRepo.GetById(id.ToString());
        public IEnumerable<Order> GetByBuyer(string buyerId)     => _orderRepo.GetAll().Where(o => o.BuyerId == buyerId);
        public IEnumerable<Order> GetByStore(string storeId)     => _orderRepo.GetAll().Where(o => o.StoreId == storeId);

        /// <summary>
        /// Places an order: validates stock, calculates total,
        /// reduces product stock, saves order.
        /// </summary>
        public Order PlaceOrder(string buyerId, string storeId, int productId, int quantity)
        {
            var product = _productRepo.GetById(productId.ToString())
                ?? throw new KeyNotFoundException($"Product {productId} not found.");
            if (product.Stock < quantity)
                throw new InvalidOperationException("Insufficient stock.");

            product.Stock -= quantity;
            _productRepo.Update(product);
            _productRepo.Save();

            var order = new Order(0, buyerId, storeId, productId, quantity, product.Price * quantity);
            _orderRepo.Add(order);
            _orderRepo.Save();
            return order;
        }

        public void UpdateStatus(int orderId, OrderStatus newStatus)
        {
            var order = _orderRepo.GetById(orderId.ToString())
                ?? throw new KeyNotFoundException($"Order {orderId} not found.");
            order.UpdateStatus(newStatus);
            _orderRepo.Update(order);
            _orderRepo.Save();
        }

        /// <summary>Returns total revenue for a store (excluding cancelled orders).</summary>
        public decimal GetRevenue(string storeId) =>
            _orderRepo.GetAll()
                      .Where(o => o.StoreId == storeId && o.Status != OrderStatus.Cancelled)
                      .Sum(o => o.TotalPrice);
    }
}
