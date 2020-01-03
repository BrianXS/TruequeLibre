using API.Entities;
using API.Utils;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Database
{
    public class TruequeLibreDbContext : IdentityDbContext<User, Role, int>
    {
        public TruequeLibreDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Offer>()
                .HasKey(table => new {table.OfferedProductId, table.ReceiverProductId});
        }

        //User Related Entities
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
        
        //Product Related Entities
        public DbSet<Product> Products { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<PictureInfo> Pictures { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Offer> Offers { get; set; }
        
    }
}