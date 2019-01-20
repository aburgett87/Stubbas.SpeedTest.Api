using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Stubias.SpeedTest.Api.Constants;
using Stubias.SpeedTest.Api.Data.Models;
using Stubias.SpeedTest.Api.Data.Services;
using Stubias.SpeedTest.Api.Models;

namespace Stubias.SpeedTest.Api.Actions
{
    public class SpeedTestResultPostAction : IPostAction<SpeedTestResult, SpeedTestResult>
    {
        private readonly IWriteTestResultOperation _operation;
        private readonly IMapper _mapper;
        public SpeedTestResultPostAction(IWriteTestResultOperation operation,
            IMapper mapper)
        {
            _mapper = mapper;
            _operation = operation;
        }

        public async Task<ActionResult<SpeedTestResult>> PostAsync(SpeedTestResult input)
        {
            var inputData = _mapper.Map<SpeedTestResult, SpeedTestResultDataModel>(input);
            await _operation.ExecuteAsync(inputData);

            return new CreatedAtRouteResult(RouteNames.GetResultAction, new { Location = input.Location}, input);
        }
    }
}