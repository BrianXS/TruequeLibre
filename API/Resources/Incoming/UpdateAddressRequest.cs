using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class UpdateAddressRequest
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public int CodigoPostal { get; set; }
        
        [Required]
        public string Neighborhood { get; set; }
        
        [Required]
        public string Street { get; set; }
        
        [Required]
        public string Number { get; set; }
        
        [Required]
        public string Division { get; set; }
        
        [Required]
        public int CityId { get; set; }
        
        public string Details { get; set; }
    }
}