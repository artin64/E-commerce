namespace Models;

public class Cart
{
    private int _userId;
    private string _storeId;
    private List<CartItem> _items;

    public Cart(int userId, string storeId)
    {
        _userId = userId;
        _storeId = storeId;
        _items = new List<CartItem>();
    }

    public int GetUserId() => _userId;
    public string GetStoreId() => _storeId;
    public List<CartItem> GetItems() => _items;

    public void AddProduct(Product product, int quantity = 1)
    {
        var existing = _items.Find(i => i.GetProductId() == product.GetId());
        if (existing != null)
            existing.IncreaseQuantity(quantity);
        else
            _items.Add(new CartItem(product.GetId(), product.GetName(), product.GetPrice(), quantity));
    }

    public void RemoveProduct(int productId) => _items.RemoveAll(i => i.GetProductId() == productId);
    public void UpdateQuantity(int productId, int quantity)
    {
        var item = _items.Find(i => i.GetProductId() == productId);
        if (item != null) item.SetQuantity(quantity);
    }
    public double GetTotal() => _items.Sum(i => i.GetSubtotal());
    public void Clear() => _items.Clear();
}

public class CartItem
{
    private int _productId;
    private string _productName;
    private double _unitPrice;
    private int _quantity;

    public CartItem(int productId, string productName, double unitPrice, int quantity)
    {
        _productId = productId;
        _productName = productName;
        _unitPrice = unitPrice;
        _quantity = quantity;
    }

    public int GetProductId() => _productId;
    public string GetProductName() => _productName;
    public double GetUnitPrice() => _unitPrice;
    public int GetQuantity() => _quantity;
    public double GetSubtotal() => _unitPrice * _quantity;

    public void SetQuantity(int quantity) => _quantity = quantity;
    public void IncreaseQuantity(int amount) => _quantity += amount;
}
