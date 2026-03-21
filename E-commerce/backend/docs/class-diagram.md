# Class Diagram — ShopPlatform

## UML Class Diagram (Mermaid)

```mermaid
classDiagram

%% ═══════════════════════════════════════════════════════
%%  MODELS LAYER
%% ═══════════════════════════════════════════════════════

class Product {
    - int _id
    - string _name
    - decimal _price
    - int _stock
    - string _category
    - string _storeId
    + int Id
    + string Name
    + decimal Price
    + int Stock
    + string Category
    + string StoreId
    + bool IsInStock
    + Product()
    + Product(id, name, price, stock, category, storeId)
    + string ToCsv()
    + Product FromCsv(csvLine) $
    + string ToString()
}

class Store {
    - string _storeId
    - string _name
    - string _ownerId
    - bool _isVerified
    - bool _isActive
    + string StoreId
    + string Name
    + string OwnerId
    + bool IsVerified
    + bool IsActive
    + Store()
    + Store(name, ownerId)
    + string ToCsv()
    + Store FromCsv(csvLine) $
    + string ToString()
}

class Order {
    - int _orderId
    - string _buyerId
    - string _storeId
    - int _productId
    - int _quantity
    - decimal _totalPrice
    - OrderStatus _status
    - DateTime _createdAt
    + int OrderId
    + string BuyerId
    + string StoreId
    + int ProductId
    + int Quantity
    + decimal TotalPrice
    + OrderStatus Status
    + DateTime CreatedAt
    + Order()
    + Order(orderId, buyerId, storeId, productId, quantity, totalPrice)
    + void UpdateStatus(newStatus)
    + string ToCsv()
    + Order FromCsv(csvLine) $
    + string ToString()
}

class User {
    - string _userId
    - string _name
    - string _email
    - string _passwordHash
    - UserRole _role
    - DateTime _createdAt
    + string UserId
    + string Name
    + string Email
    + string PasswordHash
    + UserRole Role
    + DateTime CreatedAt
    + User()
    + User(name, email, passwordHash, role)
    + string ToCsv()
    + User FromCsv(csvLine) $
    + string ToString()
}

class OrderStatus {
    <<enumeration>>
    Pending
    Confirmed
    Shipped
    Delivered
    Cancelled
}

class UserRole {
    <<enumeration>>
    Buyer
    Vendor
    SuperAdmin
}

%% ═══════════════════════════════════════════════════════
%%  DATA LAYER — Repository Pattern
%% ═══════════════════════════════════════════════════════

class IRepository~T~ {
    <<interface>>
    + IEnumerable~T~ GetAll()
    + T GetById(string id)
    + void Add(T entity)
    + void Update(T entity)
    + void Delete(string id)
    + void Save()
}

class ProductFileRepository {
    - string _filePath
    - List~Product~ _cache
    + ProductFileRepository(filePath)
    + IEnumerable~Product~ GetAll()
    + Product GetById(string id)
    + void Add(Product entity)
    + void Update(Product entity)
    + void Delete(string id)
    + void Save()
    - void EnsureFileExists()
    - void LoadFromFile()
}

class StoreFileRepository {
    - string _filePath
    - List~Store~ _cache
    + StoreFileRepository(filePath)
    + IEnumerable~Store~ GetAll()
    + Store GetById(string id)
    + void Add(Store entity)
    + void Update(Store entity)
    + void Delete(string id)
    + void Save()
    - void EnsureFileExists()
    - void LoadFromFile()
}

class OrderFileRepository {
    - string _filePath
    - List~Order~ _cache
    + OrderFileRepository(filePath)
    + IEnumerable~Order~ GetAll()
    + Order GetById(string id)
    + void Add(Order entity)
    + void Update(Order entity)
    + void Delete(string id)
    + void Save()
    - void EnsureFileExists()
    - void LoadFromFile()
}

class UserFileRepository {
    - string _filePath
    - List~User~ _cache
    + UserFileRepository(filePath)
    + IEnumerable~User~ GetAll()
    + User GetById(string id)
    + void Add(User entity)
    + void Update(User entity)
    + void Delete(string id)
    + void Save()
    - void EnsureFileExists()
    - void LoadFromFile()
}

%% ═══════════════════════════════════════════════════════
%%  SERVICES LAYER
%% ═══════════════════════════════════════════════════════

class ProductService {
    - IRepository~Product~ _repository
    + ProductService(repository)
    + IEnumerable~Product~ GetAll()
    + Product GetById(int id)
    + IEnumerable~Product~ GetByStore(storeId)
    + IEnumerable~Product~ GetByCategory(category)
    + IEnumerable~Product~ Search(keyword)
    + void AddProduct(product)
    + void UpdateProduct(product)
    + void DeleteProduct(int id)
    + bool ReduceStock(productId, qty)
    - void Validate(product)
}

class StoreService {
    - IRepository~Store~ _repository
    + StoreService(repository)
    + IEnumerable~Store~ GetAll()
    + Store GetById(string id)
    + IEnumerable~Store~ GetActive()
    + Store CreateStore(name, ownerId)
    + void UpdateStore(store)
    + void VerifyStore(storeId)
    + void DeactivateStore(storeId)
    + string GetQrCodeUrl(storeId)
}

class OrderService {
    - IRepository~Order~ _orderRepo
    - IRepository~Product~ _productRepo
    + OrderService(orderRepo, productRepo)
    + IEnumerable~Order~ GetAll()
    + Order GetById(int id)
    + IEnumerable~Order~ GetByBuyer(buyerId)
    + IEnumerable~Order~ GetByStore(storeId)
    + Order PlaceOrder(buyerId, storeId, productId, qty)
    + void UpdateStatus(orderId, newStatus)
    + decimal GetRevenue(storeId)
}

class UserService {
    - IRepository~User~ _repository
    + UserService(repository)
    + IEnumerable~User~ GetAll()
    + User GetById(string id)
    + User GetByEmail(string email)
    + User Register(name, email, password, role)
    + User Login(email, password)
    + void DeleteUser(string userId)
    - string HashPassword(password)
}

%% ═══════════════════════════════════════════════════════
%%  UI LAYER
%% ═══════════════════════════════════════════════════════

class ConsoleMenu {
    - ProductService _productService
    - StoreService _storeService
    - OrderService _orderService
    - UserService _userService
    + ConsoleMenu(productService, storeService, orderService, userService)
    + void Run()
    - void ShowAllProducts()
    - void ShowAllStores()
    - void ShowAllOrders()
    - void AddProductMenu()
    - void CreateStoreMenu()
    - void PlaceOrderMenu()
    - void RegisterMenu()
    - void LoginMenu()
    - void PrintBanner()
    - void PrintMainMenu()
    - string Prompt(label)
    - void Success(msg)
    - void Error(msg)
    - void Info(msg)
}

%% ═══════════════════════════════════════════════════════
%%  RELATIONSHIPS
%% ═══════════════════════════════════════════════════════

%% Interface Implementation (realization)
IRepository~T~ <|.. ProductFileRepository : implements
IRepository~T~ <|.. StoreFileRepository  : implements
IRepository~T~ <|.. OrderFileRepository  : implements
IRepository~T~ <|.. UserFileRepository   : implements

%% Service depends on IRepository (Dependency Inversion)
ProductService ..> IRepository~T~ : depends on
StoreService   ..> IRepository~T~ : depends on
OrderService   ..> IRepository~T~ : depends on
UserService    ..> IRepository~T~ : depends on

%% Services use Models
ProductService ..> Product : uses
StoreService   ..> Store   : uses
OrderService   ..> Order   : uses
OrderService   ..> Product : uses
UserService    ..> User    : uses

%% Repository stores Models
ProductFileRepository ..> Product : stores
StoreFileRepository   ..> Store   : stores
OrderFileRepository   ..> Order   : stores
UserFileRepository    ..> User    : stores

%% UI uses Services
ConsoleMenu ..> ProductService : uses
ConsoleMenu ..> StoreService   : uses
ConsoleMenu ..> OrderService   : uses
ConsoleMenu ..> UserService    : uses

%% Enum associations
Order   ..> OrderStatus : uses
User    ..> UserRole    : uses

%% Order references Product and Store by ID (logical association)
Order --> Product : productId
Order --> Store   : storeId
Product --> Store : storeId
```

---

## Class Descriptions

### Models Layer

| Class | Responsibility | Key Attributes |
|---|---|---|
| `Product` | Represents a store product | `_id`, `_name`, `_price`, `_stock`, `_category`, `_storeId` |
| `Store` | Represents a vendor store | `_storeId`, `_name`, `_ownerId`, `_isVerified`, `_isActive` |
| `Order` | Represents a purchase order | `_orderId`, `_buyerId`, `_productId`, `_quantity`, `_totalPrice`, `_status` |
| `User` | Represents a platform user | `_userId`, `_name`, `_email`, `_passwordHash`, `_role` |
| `OrderStatus` | Enum for order lifecycle | `Pending → Confirmed → Shipped → Delivered / Cancelled` |
| `UserRole` | Enum for access control | `Buyer`, `Vendor`, `SuperAdmin` |

### Data Layer

| Class | Responsibility |
|---|---|
| `IRepository<T>` | Interface contract for all data operations |
| `ProductFileRepository` | CSV-based CRUD for Product entities |
| `StoreFileRepository` | CSV-based CRUD for Store entities |
| `OrderFileRepository` | CSV-based CRUD for Order entities |
| `UserFileRepository` | CSV-based CRUD for User entities |

### Services Layer

| Class | Responsibility |
|---|---|
| `ProductService` | Product business logic (add, update, search, stock management) |
| `StoreService` | Store management (create, verify, deactivate, QR generation) |
| `OrderService` | Order placement, status tracking, revenue calculation |
| `UserService` | User registration, authentication (SHA-256 hashing) |

### UI Layer

| Class | Responsibility |
|---|---|
| `ConsoleMenu` | All console I/O — menus, prompts, display, formatting |
