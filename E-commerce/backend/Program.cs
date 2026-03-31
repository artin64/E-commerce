using ECommerce.Data;
using ECommerce.Services;
using ECommerce.UI;

var productRepo = new ProductFileRepository();
var storeRepo   = new StoreFileRepository();
var orderRepo   = new OrderFileRepository();
var userRepo    = new UserFileRepository();
new ConsoleMenu(new ProductService(productRepo), new StoreService(storeRepo),
    new OrderService(orderRepo, productRepo), new UserService(userRepo)).Run();
