using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stubias.SpeedTest.Api.Actions;
using Stubias.SpeedTest.Api.Models;
using Stubias.SpeedTest.Api.Data.Models;
using Stubias.SpeedTest.Api.Models.Input;
using Stubias.SpeedTest.Api.Constants;
using System.Collections.Generic;

namespace Stubias.SpeedTest.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly IGetAction<SpeedTestResultInputModel, IEnumerable<SpeedTestResult>> _getAction;
        private readonly IPostAction<SpeedTestResult, SpeedTestResult> _postAction;

        public ResultsController(IGetAction<SpeedTestResultInputModel, IEnumerable<SpeedTestResult>> getAction,
            IPostAction<SpeedTestResult, SpeedTestResult> postAction)
        {
            _getAction = getAction;
            _postAction = postAction;
        }

        [HttpGet("{location}", Name = RouteNames.GetResultAction)]
        public async Task<ActionResult<IEnumerable<SpeedTestResult>>> GetResultByLocationAsync(
            [FromQuery] SpeedTestResultInputModel location)
            => await _getAction.GetAsync(location);

        [HttpPost]
        public async Task<ActionResult<SpeedTestResult>> PostResultAsync([FromBody] SpeedTestResult input)
            => await _postAction.PostAsync(input);
    }
}