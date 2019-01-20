using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Stubias.SpeedTest.Api.Data.Models;
using Stubias.SpeedTest.Api.Data.Services;
using Stubias.SpeedTest.Api.Models;
using Stubias.SpeedTest.Api.Models.Input;

namespace Stubias.SpeedTest.Api.Actions
{
    public class SpeedTestResultGetAction : IGetAction<SpeedTestResultInputModel, IEnumerable<SpeedTestResult>>
    {
        private readonly IQueryTestResultsOperation _operation;
        private readonly IMapper _mapper;
        public SpeedTestResultGetAction(IQueryTestResultsOperation operation,
            IMapper mapper)
        {
            _mapper = mapper;
            _operation = operation;
        }

    public async Task<ActionResult<IEnumerable<SpeedTestResult>>> GetAsync(SpeedTestResultInputModel input)
    {
        var resultData = await _operation.ExecuteAsync(input.Location, input.StartDateTime, input.EndDateTime);
        var results = _mapper.Map<IEnumerable<SpeedTestResultDataModel>, IEnumerable<SpeedTestResult>>(resultData);

        return results.ToList();
    }
}
}