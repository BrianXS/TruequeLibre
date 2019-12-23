using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Repositories.Interfaces;
using API.Resources.Outgoing;
using API.Services.Database;
using Microsoft.AspNetCore.Identity;

namespace API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly TruequeLibreDbContext _dbContext;

        public UserRepository(UserManager<User> userManager, 
                              TruequeLibreDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        
        public async Task<User> FindUserByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<User> FindUserById(int id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task UpdateName(User user, string names, string lastnames)
        {
            user.Names = string.IsNullOrWhiteSpace(names.Trim()) ? user.Names : names;
            user.LastNames = string.IsNullOrWhiteSpace(lastnames.Trim()) ? user.LastNames : lastnames;
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateEmail(User user, string email)
        {
            user.Email = email;
            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> UpdatePassword(User user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        public async Task UpdateUserName(User user, string userName)
        {
            user.UserName = userName;
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateUserPhone(User user, string phone)
        {
            user.PhoneNumber = phone;
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateRefreshToken(User user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
        }

        public async Task<IList<string>> GetUserRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task CreateUser(User user, string password)
        {
            await _userManager.CreateAsync(user, password);
        }
    }
}