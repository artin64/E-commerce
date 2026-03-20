using Models;
using Data;
using Services.Interfaces;

namespace Services;

// DIP: ProductService varet nga IRepository<Product>, jo nga FileRepository konkrete.
// SRP: Vetëm logjika e biznesit për produkte.
public class ProductService : IProductService
{
    private readonly IRepository<Product> _repository;

    public ProductService(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public List<Product> GetAll(string storeId)
        => _repository.GetAll().Where(p => p.GetStoreId() == storeId).ToList();

    public Product? GetById(int id) => _repository.GetById(id);

    public void AddProduct(Product product)
    {
        _repository.Add(product);
        _repository.Save();
    }

    public void UpdateProduct(Product product)
    {
        _repository.Update(product);
        _repository.Save();
    }

    public void DeleteProduct(int id)
    {
        _repository.Delete(id);
        _repository.Save();
    }

    public List<Product> Search(string query, string storeId)
        => GetAll(storeId)
            .Where(p => p.GetName().Contains(query, StringComparison.OrdinalIgnoreCase)
                     || p.GetDescription().Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

    public List<Product> Filter(string storeId, double? minPrice, double? maxPrice, int? categoryId, double? minRating)
    {
        var products = GetAll(storeId);
        if (minPrice.HasValue) products = products.Where(p => p.GetPrice() >= minPrice).ToList();
        if (maxPrice.HasValue) products = products.Where(p => p.GetPrice() <= maxPrice).ToList();
        if (categoryId.HasValue) products = products.Where(p => p.GetCategoryId() == categoryId).ToList();
        if (minRating.HasValue) products = products.Where(p => p.GetRating() >= minRating).ToList();
        return products;
    }
}
