using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class UpdateUserNameRequest
    {
        public string Names { get; set; }
        public string LastNames { get; set; }
    }
}