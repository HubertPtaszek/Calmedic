namespace Calmedic.Domain
{
    public class Address : AuditEntity
    {
        public Address()
        {
        }

        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNo { get; set; }
        public string ApartmentNo { get; set; }
        public string PostalCode { get; set; }
        public string FullAdress { get; set; }
    }
}