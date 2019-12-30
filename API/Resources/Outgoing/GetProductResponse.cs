using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.Resources.Outgoing
{
    public class GetProductResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public UserDto User { get; set; }
        public List<string> Picutres { get; set; }
        public List<DetailDto> Details { get; set; }
        public List<QuestionDto> Question { get; set; }

        public class UserDto
        {
            public int Id { get; set; }
            public string Names { get; set; }
            public string LastNames { get; set; }
            public string UserName { get; set; }
        }

        public class DetailDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
        
        public class QuestionDto
        {
            public string Question { get; set; }
            public List<string> Answers { get; set; }
        }
    }
}