using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<PictureInfo> Pictures { get; set; }
        public List<Detail> Details { get; set; }
        public List<Question> Questions { get; set; }

        [InverseProperty("ReceiverProduct")]
        public List<Offer> IncomingOffers { get; set; }
        
        [InverseProperty("OfferedProduct")]
        public List<Offer> OutcomingOffers { get; set; }
    }
}