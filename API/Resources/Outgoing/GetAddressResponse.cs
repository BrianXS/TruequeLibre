using API.Entities;

namespace API.Resources.Outgoing
{
    public class GetAddressResponse
    {
        public int Id { get; set; }
        public int CodigoPostal { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Division { get; set; }
        public string Details { get; set; }
        public City City { get; set; }
    }
}