using API.Entities;
using API.Resources.Incoming;
using API.Resources.Outgoing;
using AutoMapper;

namespace API.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<AddQuestionToProduct, Question>();
            CreateMap<Question, GetProductResponse.QuestionDto>().ForMember(
                destination => destination.Question,
                source => 
                    source.MapFrom(src => src.Body));
        }
    }
}