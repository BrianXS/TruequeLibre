using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Offer
    {
        public int OfferedProductId { get; set; }
        public Product OfferedProduct { get; set; }
        
        public int ReceiverProductId { get; set; }
        public Product ReceiverProduct { get; set; }
    }
}