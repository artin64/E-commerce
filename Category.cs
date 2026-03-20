using Models;

namespace Services.Interfaces;

// ISP: Interface të ndara, specifike për secilin shërbim

public interface IProductService
{
    List<Product> GetAll(string storeId);
    Product? GetById(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(int id);
    List<Product> Search(string query, string storeId);
    List<Product> Filter(string storeId, double? minPrice, double? maxPrice, int? categoryId, double? minRating);
}

public interface IOrderService
{
    Order CreateOrder(int userId, string storeId, string shippingAddress, List<OrderItem> items);
    Order? GetById(int id);
    List<Order> GetUserOrders(int userId);
    List<Order> GetStoreOrders(string storeId);
    void UpdateStatus(int orderId, OrderStatus status);
}

public interface IAuthService
{
    User? Login(string email, string password);
    User Register(string name, string email, string password, string address, UserRole role);
    bool ChangePassword(int userId, string oldPassword, string newPassword);
}

public interface IStoreService
{
    Store CreateStore(string name, int ownerId);
    Store? GetById(string storeId);
    void UpdateTheme(string storeId, string theme, string primaryColor, string bgColor);
    void SetActive(string storeId, bool active);
    List<Store> GetAll();
}

public interface ICategoryService
{
    List<Category> GetByStore(string storeId);
    void AddCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(int id);
}

public interface ICartService
{
    Cart GetCart(int userId, string storeId);
    void AddProduct(int userId, string storeId, Product product, int quantity);
    void RemoveProduct(int userId, string storeId, int productId);
    void UpdateQuantity(int userId, string storeId, int productId, int quantity);
    double GetTotal(int userId, string storeId);
    void Clear(int userId, string storeId);
}

public interface IReviewService
{
    List<Review> GetByProduct(int productId);
    void AddReview(Review review);
    double GetAverageRating(int productId);
}

public interface INotificationService
{
    void Send(int userId, string message, NotificationType type);
    List<Notification> GetUnread(int userId);
    void MarkAsRead(int notificationId);
}

public interface IAnalyticsService
{
    double GetTotalRevenue(string storeId);
    List<Product> GetTopProducts(string storeId, int count);
    int GetVisitorCount(string storeId);
    int GetOrderCount(string storeId);
}
