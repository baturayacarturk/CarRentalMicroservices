using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models;

namespace UserInterface.MicroserviceCommunication.Concrete
{
    public class CredentialService : ICredentialService
    {
        private readonly MicroServiceApiAdjustment _microservice;
        private readonly UserSettings _userSettings;
        private readonly IClientAccessTokenCache _cache;
        private readonly HttpClient _httpClient;
        public CredentialService(IOptions<MicroServiceApiAdjustment> microservice, IOptions<UserSettings> userSettings, IClientAccessTokenCache cache, HttpClient httpClient)
        {
            _microservice = microservice.Value;
            _userSettings = userSettings.Value;
            _cache = cache;
            _httpClient = httpClient;
        }

        public async Task<string> GetTheToken()
        {
            var token = await _cache.GetAsync("carrentalcookie");
            if (token != null)
            {
                return token.AccessToken;
            }
            var identityServerGeneralPath = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _microservice.IdentityUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (identityServerGeneralPath.IsError)
            {

                throw identityServerGeneralPath.Exception;
            }
            var tokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = _userSettings.NormalUser.UserIdentityType,
                ClientSecret = _userSettings.NormalUser.Secret,
                Address = identityServerGeneralPath.TokenEndpoint
            };
            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
            if (newToken.IsError)
            {
                throw newToken.Exception;
            }
            await _cache.SetAsync("carrentalcookie", newToken.AccessToken, newToken.ExpiresIn);
            return newToken.AccessToken;    
        }

    }
}
