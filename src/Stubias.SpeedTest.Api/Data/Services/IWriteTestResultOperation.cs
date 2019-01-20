using System.Threading.Tasks;
using Stubias.SpeedTest.Api.Data.Models;

namespace Stubias.SpeedTest.Api.Data.Services
{
    public interface IWriteTestResultOperation
    {
        Task ExecuteAsync(SpeedTestResultDataModel testResult);
    }
}