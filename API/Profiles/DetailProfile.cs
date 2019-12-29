using API.Entities;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using AutoMapper;

namespace API.Profiles
{
    public class DetailProfile : Profile
    {
        public DetailProfile()
        {
            CreateMap<AddProductRequest.DetailDto, Detail>();
            CreateMap<Detail, GetProductResponse.DetailDto>();
            CreateMap<Detail, UpdateProductResponse.DetailDto>();
            CreateMap<UpdateProductRequest.DetailDto, Detail>();
            CreateMap<Detail, UpdateProductRequest.DetailDto>();
        }
    }
}