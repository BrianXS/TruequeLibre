using System.Linq;
using API.Entities;
using API.Resources.Outgoing;
using AutoMapper;

namespace API.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProfileInfoResponse.ProductInfo>()
                .ForMember(destination => destination.ProductPictures,
                    source => 
                        source.MapFrom(src => 
                            src.Pictures.Select(x => x.FilePath).ToList()));
        }
    }
}