

using System.Net.Http.Headers;
using UserInterface.Exceptions;
using UserInterface.MicroserviceCommunication.Abstract;

namespace UserInterface.Handlers
{
    public class CredentialHandler : DelegatingHandler
    {
        private readonly ICredentialService _credentialService;

        public CredentialHandler(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _credentialService.GetTheToken());
            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsStringAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
            }
            return response;
        }
    }
}
