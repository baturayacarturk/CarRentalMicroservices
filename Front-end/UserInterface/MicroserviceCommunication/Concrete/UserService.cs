using Microsoft.CodeAnalysis;
using System.Drawing.Text;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models;

namespace UserInterface.MicroserviceCommunication.Concrete
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {

            return await _httpClient.GetFromJsonAsync<UserViewModel>("/api/user/getuser");

        }

        
    }
}
