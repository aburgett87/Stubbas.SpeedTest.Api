using System.Collections.Generic;

namespace Stubias.SpeedTest.Api.Models.Configuration
{
    public class Auth
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
        public string TokenUrl { get; set; }
        public string AuthorizationUrl { get; set; }
        public ICollection<AuthScope> Scopes { get; set; } = new List<AuthScope>();
    }
}
