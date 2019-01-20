using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Stubias.SpeedTest.Api.Data.Models;

namespace Stubias.SpeedTest.Api.Data.Services
{
    public class WriteTestResultOperation : IWriteTestResultOperation
    {
        private readonly IDynamoDBContext _context;
        public WriteTestResultOperation(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(SpeedTestResultDataModel testResult)
        {
            await _context.SaveAsync(testResult);
        }
    }
}