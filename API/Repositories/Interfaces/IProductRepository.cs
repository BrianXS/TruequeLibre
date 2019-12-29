using System.Collections.Generic;
using API.Entities;

namespace API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        void SaveProduct(Product product);
        Product FindProductById(int id);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        List<Product> GetUserProducts(int id);
    }
}