using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class ChangePasswordRequest
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Token { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}