namespace UserInterface.Models
{
    public class MicroServiceApiAdjustment
    {
        public string IdentityUri { get; set; }
        public string BaseUriGateway { get; set; }
        public string PhotoUri { get; set; }
        public ServiceApi Catalog { get; set; }
        public ServiceApi Photo { get; set; }
        public ServiceApi Basket { get; set; }
        public ServiceApi Discount { get; set; }
        public ServiceApi Payment{ get; set; }
        public ServiceApi Order { get; set; }
    }
    public class ServiceApi
    {
        public string Path { get; set; }
    }
}
