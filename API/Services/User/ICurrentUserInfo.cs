using System.Threading.Tasks;
using API.Resources.Outgoing;

namespace API.Services.User
{
    public interface ICurrentUserInfo
    {
        Task<int> GetCurrentUserId();
        Task<Entities.User> GetCurrentUser();
        Task<ProfileResponse> GetCurrentUserResource();
        Task<UpdateUserResponse> GetCurrentUserUpdateResource();
    }
}