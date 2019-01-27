using System.Threading.Tasks;

namespace Stubias.SpeedTest.Api.Integration.Helpers
{
    public interface IAccessTokenFactory
    {
        Task<string> GetAuthTokenAsync();
    }
}
