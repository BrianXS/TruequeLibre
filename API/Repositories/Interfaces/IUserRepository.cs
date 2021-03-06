using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Resources.Outgoing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindUserById(int id);
        Task<User> FindUserByUsername(string username);
        ProfileResponse FindProfileResponseByName(string userName);
        ProfileResponse FindProfileResponseById(int id);
        Task UpdateName(User user, string names, string lastnames);
        Task UpdateEmail(User user, string email);
        Task<bool> UpdatePassword(User user, string oldPassword, string newPassword);
        Task UpdateUserName(User user, string userName);
        Task UpdateUserPhone(User user, string Phone);
        Task UpdateRefreshToken(User user, string refreshToken);
        Task<IList<string>> GetUserRoles(User user);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
        Task CreateUser(User user, string password);
    }
}