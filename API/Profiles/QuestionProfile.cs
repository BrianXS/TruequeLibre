using API.Entities;
using API.Resources.Incoming;
using AutoMapper;

namespace API.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<AddQuestionToProduct, Question>();
        }
    }
}