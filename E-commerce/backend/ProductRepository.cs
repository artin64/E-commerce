using System.Collections.Generic;
using ECommerce.Models;

namespace ECommerce.Repositories.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        void Save();
        int GetNextId();
    }
}
