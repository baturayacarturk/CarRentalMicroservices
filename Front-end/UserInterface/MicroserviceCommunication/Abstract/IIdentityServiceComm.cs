using CarRental.Shared.Dtos;
using IdentityModel.Client;
using UserInterface.Models;

namespace UserInterface.MicroserviceCommunication.Abstract
{
    public interface  IIdentityServiceComm
    {
        Task<ResponseDto<bool>> SignIn(SignInCredentials singInCredentials);
        Task<TokenResponse> GetRefreshToken();
        Task RevokeRefreshToken();
        Task<ResponseDto<bool>> SignUp(UserSignupModel userSignUpModel);
    }
}
