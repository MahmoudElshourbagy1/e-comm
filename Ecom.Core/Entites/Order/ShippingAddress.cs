namespace Ecom.Core.Entites.Order
{
    public class ShippingAddress :BaseEntity<int>
    {
        public ShippingAddress()
        {
        }

        public ShippingAddress(string fristName, string lastName, string city, string zipCode, string street, string state)
        {
            FristName = fristName;
            LastName = lastName;
            City = city;
            ZipCode = zipCode;
            Street = street;
            State = state;
        }

        public string FristName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
    }
}