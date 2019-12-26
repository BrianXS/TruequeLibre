using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Repositories.Implementations;
using API.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Interfaces
{
    public class ProductRepository : IProductRepository
    {
        private readonly TruequeLibreDbContext _dbContext;

        public ProductRepository(TruequeLibreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void SaveProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }

        public Product GetProductById(int id)
        {
            var result = _dbContext.Products.Where(x => x.Id == id)
                .Include(x => x.User)
                .Include(x => x.Pictures)
                .Include(x => x.Details);
            
            return result.FirstOrDefault();
        }

        public void UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
        }

        public void DeleteProduct(int id)
        {
            _dbContext.Remove(_dbContext.Products.FirstOrDefault(x => x.Id == id));
        }

        public List<Product> GetUserProducts(int id)
        {
            return _dbContext.Products.Where(x => x.UserId == id)
                .Include(x => x.Pictures).ToList();
        }
    }
}