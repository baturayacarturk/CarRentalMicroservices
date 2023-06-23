using CarRental.Common.Dtos;
using CarRental.Shared.Dtos;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NuGet.Protocol;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using UserInterface.Handlers;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models;

namespace UserInterface.MicroserviceCommunication.Concrete
{
    public class IdentityServiceComm : IIdentityServiceComm
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserSettings _userSettings;
        private readonly MicroServiceApiAdjustment _microserviceCommunication;
        private readonly ICredentialService _credentialService;
        public IdentityServiceComm(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<UserSettings> userSettings, IOptions<MicroServiceApiAdjustment> microserviceCommunication, ICredentialService credentialService)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _userSettings = userSettings.Value;
            _microserviceCommunication = microserviceCommunication
                .Value;
            _credentialService = credentialService;
        }

        public async Task<TokenResponse> GetRefreshToken()
        {
            var identityServerGeneralPath = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _microserviceCommunication.IdentityUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (identityServerGeneralPath.IsError)
            {
                throw identityServerGeneralPath.Exception;
            }
            var cookieRefreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            RefreshTokenRequest request = new()
            {
                ClientId = _userSettings.ClientForUser.UserIdentityType,
                ClientSecret = _userSettings.ClientForUser.Secret,
                RefreshToken = cookieRefreshToken,
                Address = identityServerGeneralPath.TokenEndpoint
            };
            var token = await _httpClient.RequestRefreshTokenAsync(request);
            if (token.IsError)
            {
                return null;//log maybe
            }
            
            var authentication =(new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name = OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
            new AuthenticationToken{Name = OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
            new AuthenticationToken{Name = OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
             });
            var result = await _httpContextAccessor.HttpContext.AuthenticateAsync();
            var properties = result.Properties;
            properties.StoreTokens(authentication);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,result.Principal,properties);
            return token;
        }

        public async Task RevokeRefreshToken()
        {
            var identityServerGeneralPath = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _microserviceCommunication.IdentityUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (identityServerGeneralPath.IsError)
            {
                throw identityServerGeneralPath.Exception;
            }
            var cookieToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            TokenRevocationRequest revocation = new()
            {
                ClientId = _userSettings.ClientForUser.UserIdentityType,
                ClientSecret = _userSettings.ClientForUser.Secret,
                Address = identityServerGeneralPath.RevocationEndpoint,

                Token = cookieToken,
                TokenTypeHint = "refresh_token"
            };
            await _httpClient.RevokeTokenAsync(revocation);
        }

        public async Task<ResponseDto<bool>> SignIn(SignInCredentials singInCredentials)
        {
            var identityServerGeneralPath = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _microserviceCommunication.IdentityUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (identityServerGeneralPath.IsError)
            {
                throw identityServerGeneralPath.Exception;
            }
            var passwordRequest = new PasswordTokenRequest
            {
                ClientId = _userSettings.ClientForUser.UserIdentityType,//ClientForUser.UserIdentityType,
                ClientSecret = _userSettings.ClientForUser.Secret,
                UserName = singInCredentials.Email,
                Password = singInCredentials.Password,
                Address = identityServerGeneralPath.TokenEndpoint
            };
            var createdToken = await _httpClient.RequestPasswordTokenAsync(passwordRequest);
            if (createdToken.IsError)
            {
                var response = await createdToken.HttpResponse.Content.ReadAsStringAsync();
                var error = JsonSerializer.Deserialize<ErrorWrapper>(response,new JsonSerializerOptions { PropertyNameCaseInsensitive=true});
                return ResponseDto<bool>.Fail(error.Errors, 400);
            }
            var userInformationRequest = new UserInfoRequest
            {
                Token = createdToken.AccessToken,
                Address = identityServerGeneralPath.UserInfoEndpoint,
            };

            var userInformation = await _httpClient.GetUserInfoAsync(userInformationRequest);
            if (userInformation.IsError)
            {
                throw userInformation.Exception;
            }

            ClaimsIdentity claims = new ClaimsIdentity(userInformation.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            ClaimsPrincipal principal = new ClaimsPrincipal(claims);
            var auth = new AuthenticationProperties();
            auth.StoreTokens(new List<AuthenticationToken>()
            {new AuthenticationToken{Name = OpenIdConnectParameterNames.AccessToken,Value=createdToken.AccessToken},
            new AuthenticationToken{Name = OpenIdConnectParameterNames.RefreshToken,Value=createdToken.RefreshToken},
            new AuthenticationToken{Name = OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(createdToken.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
             });
            auth.IsPersistent = singInCredentials.RememberMe;
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, auth);
            return ResponseDto<bool>.Success(200);
        }
        public async Task<ResponseDto<bool>> SignUp(UserSignupModel userSignUpModel)
        {

            var identityServerGeneralPath = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _microserviceCommunication.IdentityUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (identityServerGeneralPath.IsError)
            {
                throw identityServerGeneralPath.Exception;
            }

            var token = await _credentialService.GetTheToken();
            _httpClient.SetBearerToken(token); 

            var response = await _httpClient.PostAsJsonAsync<UserSignupModel>("http://localhost:5001/api/user/signup", userSignUpModel);

            if (response.IsSuccessStatusCode)
            {
                return ResponseDto<bool>.Success(200);

            }

            return ResponseDto<bool>.Fail("Hata oluştu. Tekrar deneyiniz",400);



        }
    }
}
