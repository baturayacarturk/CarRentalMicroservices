namespace UserInterface.MicroserviceCommunication.Abstract
{
    public interface ICredentialService
    {
        Task<String> GetTheToken();
    }
}
