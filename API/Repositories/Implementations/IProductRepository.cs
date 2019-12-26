using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Repositories.Implementations
{
    public interface IProductRepository
    {
        void SaveProduct(Product product);
        Product GetProductById(int id);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        List<Product> GetUserProducts(int id);
    }
}