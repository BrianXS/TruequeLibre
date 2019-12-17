using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Database
{
    public class TruequeLibreDbContext : IdentityDbContext<User, Role, int>
    {
        public TruequeLibreDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}