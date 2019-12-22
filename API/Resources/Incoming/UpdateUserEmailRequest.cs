using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class UpdateUserEmailRequest
    {
        [Required]
        public string Email { get; set; }
    }
}