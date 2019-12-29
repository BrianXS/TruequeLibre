using System.Collections.Generic;
using API.Entities;
using API.Repositories.Interfaces;
using API.Services.Database;

namespace API.Repositories.Implementations
{
    public class DetailRepository : IDetailRepository
    {
        private readonly TruequeLibreDbContext _dbContext;

        public DetailRepository(TruequeLibreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void RemoveRange(List<Detail> details)
        {
            _dbContext.Details.RemoveRange(details);
            _dbContext.SaveChanges();
        }

        public void AddRange(List<Detail> details)
        {
            _dbContext.Details.AddRange(details);
            _dbContext.SaveChanges();
        }
    }
}