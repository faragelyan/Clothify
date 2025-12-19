namespace Clothify.Domain.Entities
{
    public class Address
    {
        public Guid AddressID { get; set; }
        public string FullAddress { get; set; }
        public string AddressType { get; set; }
        public Guid UserID { get; set; }
    }
}
