using API.Entities;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using API.Utils;
using AutoMapper;

namespace API.Profiles
{
    public class PictureProfile : Profile
    {
        public PictureProfile()
        {
            CreateMap<AddProductRequest.PictureDto, PictureInfo>().ForMember(
                    destination => destination.FileName,
                    origin => 
                        origin.MapFrom(src =>  $"{src.Name}{src.FileType}"));
            
            CreateMap<UpdateProductRequest.PictureDto, PictureInfo>().ForMember(
                destination => destination.FileName,
                origin => 
                    origin.MapFrom(src =>  $"{src.Name}{src.FileType}"));

            CreateMap<PictureInfo, UpdateProductResponse.PictureDto>().ForMember(
                destination => destination.Content,
                destiny => 
                    destiny.MapFrom(src => Base64ToFile.ConvertToBase64(src.FileName)))
                .ForMember(destination => destination.Name,
                    origin => 
                        origin.MapFrom(src => src.FileName));


            CreateMap<PictureInfo, UpdateProductRequest.PictureDto>().ForMember(
                destination => destination.Content,
                origin => 
                    origin.MapFrom(src => Base64ToFile.ConvertToBase64(src.FileName)))
                .ForMember(destiantion => destiantion.Name,
                    origin => 
                        origin.MapFrom(src => src.FileName));
        }
    }
}