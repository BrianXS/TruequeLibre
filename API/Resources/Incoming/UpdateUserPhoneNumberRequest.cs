using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class UpdateUserPhoneNumberRequest
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}