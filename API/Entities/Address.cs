namespace API.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int CodigoPostal { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Division { get; set; }
        public string Details { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}