using CarRental.Services.Order.Domain.Core;

namespace CarRental.Services.Order.Domain.OrderAggregate
{
    public class Address : ValueObject
    {
        public Address(string provience, string district, string street, string zipCode, string line)
        {
            Provience = provience;
            District = district;
            Street = street;
            ZipCode = zipCode;
            Line = line;
        }
        //iş kuralları

        public string Provience { get;  set; }//il
        public string District { get;  set; }//ilce
        public string Street { get;  set; }
        public string ZipCode { get;  set; }
        public string Line { get;  set; }//adress satırı

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Provience;
            yield return District;
            yield return Street;
            yield return ZipCode;
            yield return Line;
        }
    }
}
