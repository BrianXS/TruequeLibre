using System.Collections.Generic;

namespace API.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Answer { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}