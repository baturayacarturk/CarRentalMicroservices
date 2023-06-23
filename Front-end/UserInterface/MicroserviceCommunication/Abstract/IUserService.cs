using UserInterface.Models;

namespace UserInterface.MicroserviceCommunication.Abstract
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
        
    }
}
