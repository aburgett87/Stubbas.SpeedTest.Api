using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stubias.SpeedTest.Api.Models.Configuration;

namespace Stubias.SpeedTest.Api.Integration.Helpers
{
    public class AccessTokenFactory : IAccessTokenFactory
    {
        private readonly Auth _authOptions;
        private readonly IConfiguration _configuration;
        public AccessTokenFactory(IOptions<Auth> authOptions,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _authOptions = authOptions.Value;
        }

        public async Task<string> GetAuthTokenAsync()
        {
            var discoClient = new HttpClient();
            var discoveryDoc = await discoClient.GetDiscoveryDocumentAsync(_authOptions.Authority);
            var token = await discoClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDoc.TokenEndpoint,
                ClientId = _configuration["Test:ClientId"],
                ClientSecret = _configuration["Test:ClientSecret"],
                Scope = string.Join(' ', _authOptions.Scopes.Select(s => s.Name)),
                Parameters = { { "audience", _authOptions.Audience } }
            });
            return token.AccessToken;
        }
    }
}
