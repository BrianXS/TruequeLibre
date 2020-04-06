using System.Collections.Generic;
using API.Entities;

namespace API.Resources.Outgoing
{
    public class ProfileResponse
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ProfilePicturePath { get; set; }
        public List<ProductInfo> Products { get; set; }
        public List<AddressInfo> Addresses { get; set; }

        public class ProductInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<string> ProductPictures { get; set; }
        }
        
        public class AddressInfo
        {
            public int Id { get; set; }
            public int CodigoPostal { get; set; }
            public string Neighborhood { get; set; }
            public string Street { get; set; }
            public string Number { get; set; }
            public string Division { get; set; }
            public string Details { get; set; }
            public string CityName { get; set; }
        }
    }
}