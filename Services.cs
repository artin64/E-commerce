using Models;
using Data;
using Services.Interfaces;

namespace Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _repository;
    private readonly INotificationService _notificationService;

    public OrderService(IRepository<Order> repository, INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public Order CreateOrder(int userId, string storeId, string shippingAddress, List<OrderItem> items)
    {
        int id = _repository.GetAll().Count + 1;
        var order = new Order(id, userId, storeId, shippingAddress);
        foreach (var item in items)
            order.AddItem(item);
        _repository.Add(order);
        _repository.Save();
        _notificationService.Send(userId, $"Porosia #{id} u krijua me sukses!", NotificationType.Order);
        return order;
    }

    public Order? GetById(int id) => _repository.GetById(id);

    public List<Order> GetUserOrders(int userId)
        => _repository.GetAll().Where(o => o.GetUserId() == userId).ToList();

    public List<Order> GetStoreOrders(string storeId)
        => _repository.GetAll().Where(o => o.GetStoreId() == storeId).ToList();

    public void UpdateStatus(int orderId, OrderStatus status)
    {
        var order = _repository.GetById(orderId);
        if (order != null)
        {
            order.SetStatus(status);
            _repository.Update(order);
            _repository.Save();
            _notificationService.Send(order.GetUserId(), $"Statusi i porosisë #{orderId}: {status}", NotificationType.Order);
        }
    }
}
