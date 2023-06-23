namespace UserInterface.Models.Orders
{
    public class AddressCreateInput
    {
        public string Provience { get; set; }//il
        public string District { get; set; }//ilce
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }//adress satırı
    }
}
