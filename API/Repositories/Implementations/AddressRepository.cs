using System.Collections.Generic;
using System.Linq;
using API.Entities;
using API.Repositories.Interfaces;
using API.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly TruequeLibreDbContext _dbContext;

        public AddressRepository(TruequeLibreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public Address GetAddress(int id)
        {
            return _dbContext.Addresses.FirstOrDefault(x => x.Id == id);
        }

        public List<Address> GetUserAddresses(int id)
        {
            return _dbContext.Addresses.Where(x => x.UserId == id).Include(x => x.City).ToList();
        }

        public Address GetAddressWithCity(int id)
        {
            return _dbContext.Addresses.Where(x => x.Id == id).Include(x => x.City).FirstOrDefault();
        }

        public void SaveAddress(Address address)
        {
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();
        }

        public void EditAddress(Address address)
        {
            _dbContext.Addresses.Update(address);
            _dbContext.SaveChanges();
        }

        public void DeleteAddress(int id)
        {
            _dbContext.Addresses.Remove(_dbContext.Addresses.First(x => x.Id == id));
            _dbContext.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _dbContext.Addresses.Any(x => x.Id == id);
        }
    }
}