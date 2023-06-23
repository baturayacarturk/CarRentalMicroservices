using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using UserInterface.Exceptions;
using UserInterface.MicroserviceCommunication.Abstract;

namespace UserInterface.Handlers
{
    public class PasswordTokenHandler:DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServiceComm _identityServiceComm;
        private readonly ILogger<PasswordTokenHandler> _logger;

        public PasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityServiceComm identityServiceComm, ILogger<PasswordTokenHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityServiceComm = identityServiceComm;
            _logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            request.Headers.Authorization=new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",accessToken);
            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var sameRequest = await _identityServiceComm.GetRefreshToken();
                if(sameRequest != null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", sameRequest.AccessToken);
                    response = await base.SendAsync(request, cancellationToken);
    
                }
            }
             if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsStringAsync();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();

            }
            return response;
        }
    }
}
