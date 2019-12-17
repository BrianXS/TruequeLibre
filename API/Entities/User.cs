using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class User : IdentityUser<int>
    {
        public string RefreshToken { get; set; }
        public List<Address> Addresses { get; set; }
    }
}