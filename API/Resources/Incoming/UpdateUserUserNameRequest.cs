using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class UpdateUserUserNameRequest
    {
        [Required]
        public string UserName { get; set; }
    }
}