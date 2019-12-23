using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class AddAddressRequest
    {
        [Required]
        public string Neighborhood { get; set; }
        
        [Required]
        public string Street { get; set; }
        
        [Required]
        public string Number { get; set; }

        [Required]
        public int CityId { get; set; }
        
        public int CodigoPostal { get; set; }
        public string Division { get; set; }
        public string Details { get; set; }
    }
}