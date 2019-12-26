using API.Entities;
using API.Resources.Incoming;
using AutoMapper;

namespace API.Profiles
{
    public class PictureProfile : Profile
    {
        public PictureProfile()
        {
            CreateMap<AddProductRequest.PictureDto, PictureInfo>().ForMember(
                    destination => destination.FileName,
                    source => 
                        source.MapFrom(src =>  $"{src.Name}{src.FileType}"));
        }
    }
}