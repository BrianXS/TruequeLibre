using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class UpdateUserPasswordRequest
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}