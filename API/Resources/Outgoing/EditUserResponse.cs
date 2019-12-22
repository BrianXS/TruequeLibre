using System.Collections.Generic;
using API.Entities;

namespace API.Resources.Outgoing
{
    public class EditUserResponse
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string PhoneNumber { get; set; }
        public List<Address> Addresses { get; set; }
    }
}