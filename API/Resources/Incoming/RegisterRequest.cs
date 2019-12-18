using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class RegisterRequest
    {
        [Required, MinLength(3), MaxLength(30)]
        public string Names { get; set; }
        
        [Required, MinLength(3), MaxLength(30)]
        public string LastNames { get; set; }
        
        [Required, MinLength(5), MaxLength(20)]
        public string UserName { get; set; }
        
        [Required, MinLength(6), MaxLength(40)]
        public string Email { get; set; }
        
        [Required, MinLength(6), MaxLength(30)]
        public string Password { get; set; }
    }
}