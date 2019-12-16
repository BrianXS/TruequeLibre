using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Services.Database
{
    public class SeedData
    {
        public static async Task InitializeMethods(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!roleManager.Roles.Any())
                await InitializeRoles(roleManager);

            if (!userManager.Users.Any())
                await InitializeUsers(userManager);
        }

        private static async Task InitializeUsers(UserManager<User> userManager)
        {
            var user = new User
            {
                UserName = "trocador",
                Email = "trocador@truquelibre.com.mx",
            };

            var result = await userManager.CreateAsync(user, "strong-password");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Constants.Roles.Admin);
            }
        }

        private static async Task InitializeRoles(RoleManager<Role> roleManager)
        {
            var admin = new Role { Name = Constants.Roles.Admin };
            var mod = new Role { Name = Constants.Roles.Mod };
            var normalUser = new Role { Name = Constants.Roles.NormalUser };

            await roleManager.CreateAsync(admin);
            await roleManager.CreateAsync(mod);
            await roleManager.CreateAsync(normalUser);
        }
    }
}