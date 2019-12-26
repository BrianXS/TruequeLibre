using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Database
{
    public class TruequeLibreDbContext : IdentityDbContext<User, Role, int>
    {
        public TruequeLibreDbContext(DbContextOptions options) : base(options) { }

        //User Related Entities (i.e. User -> Addresses, Addresses -> cities)
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
        
        //Product Related Entities
        public DbSet<Product> Products { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<PictureInfo> Pictures { get; set; }
    }
}