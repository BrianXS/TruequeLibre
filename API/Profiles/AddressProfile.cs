using API.Entities;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using AutoMapper;

namespace API.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddAddressRequest, Address>();
            CreateMap<Address, GetAddressResponse>();
            CreateMap<UpdateAddressRequest, Address>();
            
            CreateMap<Address, GetProfileResponse.AddressInfo>().ForMember(
                destination => destination.CityName,
                source => 
                    source.MapFrom(src => src.City.Name));
        }
    }
}