using Models;
using Data;
using Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _repository;

    public AuthService(IRepository<User> repository) => _repository = repository;

    public User? Login(string email, string password)
    {
        string hash = HashPassword(password);
        return _repository.GetAll().FirstOrDefault(u => u.GetEmail() == email && u.GetPasswordHash() == hash);
    }

    public User Register(string name, string email, string password, string address, UserRole role)
    {
        int id = _repository.GetAll().Count + 1;
        var user = new User(id, email, HashPassword(password), name, address, role);
        _repository.Add(user);
        _repository.Save();
        return user;
    }

    public bool ChangePassword(int userId, string oldPassword, string newPassword)
    {
        var user = _repository.GetById(userId);
        if (user == null || user.GetPasswordHash() != HashPassword(oldPassword)) return false;
        var updated = new User(userId, user.GetEmail(), HashPassword(newPassword), user.GetName(), user.GetAddress(), user.GetRole());
        _repository.Update(updated);
        _repository.Save();
        return true;
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}

public class StoreService : IStoreService
{
    private readonly IRepository<Store> _repository;

    public StoreService(IRepository<Store> repository) => _repository = repository;

    public Store CreateStore(string name, int ownerId)
    {
        string id = Guid.NewGuid().ToString("N")[..8].ToUpper();
        var store = new Store(id, name, ownerId);
        _repository.Add(store);
        _repository.Save();
        return store;
    }

    public Store? GetById(string storeId)
        => _repository.GetAll().FirstOrDefault(s => s.GetId() == storeId);

    public void UpdateTheme(string storeId, string theme, string primaryColor, string bgColor)
    {
        var store = GetById(storeId);
        if (store == null) return;
        store.SetTheme(theme);
        store.SetPrimaryColor(primaryColor);
        store.SetBackgroundColor(bgColor);
        _repository.Update(store);
        _repository.Save();
    }

    public void SetActive(string storeId, bool active)
    {
        var store = GetById(storeId);
        if (store == null) return;
        store.SetActive(active);
        _repository.Update(store);
        _repository.Save();
    }

    public List<Store> GetAll() => _repository.GetAll();
}

public class CartService : ICartService
{
    private readonly Dictionary<string, Cart> _carts = new();

    private string Key(int userId, string storeId) => $"{userId}_{storeId}";

    public Cart GetCart(int userId, string storeId)
    {
        string key = Key(userId, storeId);
        if (!_carts.ContainsKey(key))
            _carts[key] = new Cart(userId, storeId);
        return _carts[key];
    }

    public void AddProduct(int userId, string storeId, Product product, int quantity)
        => GetCart(userId, storeId).AddProduct(product, quantity);

    public void RemoveProduct(int userId, string storeId, int productId)
        => GetCart(userId, storeId).RemoveProduct(productId);

    public void UpdateQuantity(int userId, string storeId, int productId, int quantity)
        => GetCart(userId, storeId).UpdateQuantity(productId, quantity);

    public double GetTotal(int userId, string storeId)
        => GetCart(userId, storeId).GetTotal();

    public void Clear(int userId, string storeId)
        => GetCart(userId, storeId).Clear();
}

public class ReviewService : IReviewService
{
    private readonly IRepository<Review> _repository;
    private readonly IProductService _productService;

    public ReviewService(IRepository<Review> repository, IProductService productService)
    {
        _repository = repository;
        _productService = productService;
    }

    public List<Review> GetByProduct(int productId)
        => _repository.GetAll().Where(r => r.GetProductId() == productId).ToList();

    public void AddReview(Review review)
    {
        _repository.Add(review);
        _repository.Save();
        double avg = GetAverageRating(review.GetProductId());
        var product = _productService.GetById(review.GetProductId());
        int count = GetByProduct(review.GetProductId()).Count;
        product?.UpdateRating(avg, count);
    }

    public double GetAverageRating(int productId)
    {
        var reviews = GetByProduct(productId);
        return reviews.Count == 0 ? 0 : reviews.Average(r => r.GetRating());
    }
}

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _repository;

    public NotificationService(IRepository<Notification> repository) => _repository = repository;

    public void Send(int userId, string message, NotificationType type)
    {
        int id = _repository.GetAll().Count + 1;
        var notification = new Notification(id, userId, message, type);
        _repository.Add(notification);
        _repository.Save();
    }

    public List<Notification> GetUnread(int userId)
        => _repository.GetAll().Where(n => n.GetUserId() == userId && !n.IsRead()).ToList();

    public void MarkAsRead(int notificationId)
    {
        var notification = _repository.GetById(notificationId);
        notification?.MarkAsRead();
        _repository.Save();
    }
}

public class AnalyticsService : IAnalyticsService
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;

    public AnalyticsService(IOrderService orderService, IProductService productService)
    {
        _orderService = orderService;
        _productService = productService;
    }

    public double GetTotalRevenue(string storeId)
        => _orderService.GetStoreOrders(storeId)
            .Where(o => o.GetStatus() != OrderStatus.Cancelled)
            .Sum(o => o.GetTotal());

    public List<Product> GetTopProducts(string storeId, int count)
        => _productService.GetAll(storeId)
            .OrderByDescending(p => p.GetRating())
            .Take(count)
            .ToList();

    public int GetVisitorCount(string storeId) => new Random().Next(100, 5000); // Placeholder

    public int GetOrderCount(string storeId)
        => _orderService.GetStoreOrders(storeId).Count;
}
