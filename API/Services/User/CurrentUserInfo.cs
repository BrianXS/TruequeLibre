using System.Threading.Tasks;
using API.Repositories.Interfaces;
using API.Resources.Outgoing;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace API.Services.User
{
    public class CurrentUserInfo : ICurrentUserInfo
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CurrentUserInfo(IHttpContextAccessor contextAccessor,
                               IUserRepository userRepository,
                               IMapper mapper)
        {
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<int> GetCurrentUserId()
        {
            var userName = _contextAccessor.HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByUsername(userName);
            return userInfo.Id;
        }

        public async Task<Entities.User> GetCurrentUser()
        {
            var userName = _contextAccessor.HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByUsername(userName);
            return userInfo;
        }

        public async Task<ProfileResponse> GetCurrentUserResource()
        {
            var userName = _contextAccessor.HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByUsername(userName);
            return _mapper.Map<ProfileResponse>(userInfo);
            
        }
        
        public async Task<UpdateUserResponse> GetCurrentUserUpdateResource()
        {
            var userName = _contextAccessor.HttpContext.User.Identity.Name;
            var userInfo = await _userRepository.FindUserByUsername(userName);
            return _mapper.Map<UpdateUserResponse>(userInfo);
        }
    }
}