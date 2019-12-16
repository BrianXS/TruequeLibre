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
    }
}