namespace ExamenAdam.Entities
{
    public class Address : Entity
    {
        public string StreetName { get; set; } = null!;
        public string StreetNumber { get; set; } = null!;
        public string PostalBus { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
