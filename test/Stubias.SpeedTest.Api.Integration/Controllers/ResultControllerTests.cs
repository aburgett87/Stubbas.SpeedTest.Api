using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Stubias.SpeedTest.Api.Models;
using Xunit;

namespace Stubias.SpeedTest.Api.Integration.Controllers
{
    public class ResultControllerTests
        : IClassFixture<SpeedTestWebApplicationFactory<Startup>>
    {
        private readonly SpeedTestWebApplicationFactory<Startup> _factory;
        public ResultControllerTests(SpeedTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostSpeedTestResult()
        {
            //Arrange
            var url = "/api/results";
            var executionTime = DateTime.UtcNow;
            var input = new SpeedTestResult
            {
                Location = "test-home",
                ExecutionDateTime = executionTime,
                AverageDownloadSpeed = 22.1m,
                MaximumDownloadSpeed = 50.2m,
                AverageUploadSpeed = 10.1m,
                MaximumUploadSpeed = 12.1m,
                Latency = 40.1m,
                TestServerName = "test-sever",
                NodeName = "node"
            };
            var client = _factory.CreateClient();

            //Act
            var postResponse = await client.PostAsJsonAsync(url, input);

            //Assert
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        }

        [Fact]
        public async Task GetSpeedTestResult()
        {
            var url = "/api/results";
            var executionTime = DateTime.Parse("2019-01-01T00:00:00.000Z");
            var location = "test-home";

            var client = _factory.CreateClient();
            var query = new Dictionary<string, string>
            {
                {"startDateTime", executionTime.ToString() },
                {"endDateTime", executionTime.ToString() }
            };

            //Act
            var getResponse = await client.GetAsync(QueryHelpers.AddQueryString($"{url}/{location}", query));
            var contentString = await getResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<SpeedTestResult>>(contentString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.True(result.Count == 1);
            Assert.Contains(result, r => r.Location == "test-home");
        }
    }
}