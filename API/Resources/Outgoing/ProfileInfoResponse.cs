using System.Collections.Generic;
using API.Entities;

namespace API.Resources.Outgoing
{
    public class ProfileInfoResponse
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ProfilePicturePath { get; set; }
        public List<ProductInfo> Products { get; set; }

        public class ProductInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<string> ProductPictures { get; set; }
        }
    }
}