using System.Linq;
using API.Entities;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using AutoMapper;

namespace API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequest, User>();
            CreateMap<User, EditUserResponse>();
            CreateMap<User, GetProductResponse.UserDto>();
            
            CreateMap<User, GetProfileResponse>()
                .ForMember(destination => destination.FullName,
                    source => 
                        source.MapFrom(user => $"{user.Names} {user.LastNames}"))
                .ForMember(destination => destination.Products,
                    entity => 
                        entity.MapFrom(src => src.Products));
        }
    }
}