using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}