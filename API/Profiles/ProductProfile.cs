using System.Linq;
using API.Entities;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;

namespace API.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductRequest, Product>().ForMember(
                dest => dest.Details,
                source => 
                    source.MapFrom(src => src.Details));

            CreateMap<Product, GetProfileResponse.ProductInfo>().ForMember(
                destination => destination.ProductPictures,
                entity => 
                    entity.MapFrom(src => src.Pictures.Select(picture => picture.FilePath).ToList()));


            CreateMap<Product, GetProductResponse>().ForMember(
                destination => destination.Details,
                entity => 
                    entity.MapFrom(src => src.Details))
                .ForMember(destination => destination.Picutres,
                    entity => 
                        entity.MapFrom(src => src.Pictures.Select(x => x.FilePath).ToList()));
        }
    }
}