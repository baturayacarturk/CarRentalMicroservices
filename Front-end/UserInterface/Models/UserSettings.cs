namespace UserInterface.Models
{
    public class UserSettings
    {
        public Client NormalUser { get; set; }
        public Client ClientForUser { get; set; }
    }
    public class Client
    {
        public string UserIdentityType { get; set; }
        public string Secret { get; set; }
    }
}
