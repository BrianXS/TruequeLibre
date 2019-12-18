using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class User : IdentityUser<int>
    {
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string RefreshToken { get; set; }
        public List<Address> Addresses { get; set; }
    }
}