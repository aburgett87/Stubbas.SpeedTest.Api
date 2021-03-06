using System;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stubias.SpeedTest.Api.Data.Models;
using Stubias.SpeedTest.Api.Integration.Helpers;

namespace Stubias.SpeedTest.Api.Integration
{
    public class SpeedTestWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var dynamoDbUrl = TestAndSetEnvironmentVariable("AWS__ServiceURL", "http://127.0.0.1:8000");
            builder.ConfigureServices(services =>
            {
                var awsOptions = new AWSOptions();
                awsOptions.DefaultClientConfig.ServiceURL = dynamoDbUrl;
                services.AddDefaultAWSOptions(awsOptions)
                    .AddAWSService<IAmazonDynamoDB>(awsOptions)
                    .AddScoped<IDynamoDBContext, DynamoDBContext>()
                    .AddScoped<DynamoDBContextConfig>(
                        _ => new DynamoDBContextConfig { TableNamePrefix = "Development" }
                    );

                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<SpeedTestWebApplicationFactory<TStartup>>>();
                    var dynamoDb = scope.ServiceProvider.GetRequiredService<IAmazonDynamoDB>();
                    var dynamoDbContext = scope.ServiceProvider.GetRequiredService<IDynamoDBContext>();
                    logger.LogInformation($"Seeding data for dynamodb instance at {dynamoDb.Config.ServiceURL}");
                    var input = new SpeedTestResultDataModel
                    {
                        Location = "test-home",
                        ExecutionDateTime = DateTime.Parse("2019-01-01T00:00:00.000Z"),
                        AverageDownloadSpeed = 22.1m,
                        MaximumDownloadSpeed = 50.2m,
                        AverageUploadSpeed = 10.1m,
                        MaximumUploadSpeed = 12.1m,
                        Latency = 40.1m,
                        TestServerName = "test-sever",
                        NodeName = "node"
                    };
                    dynamoDbContext.SaveAsync(input).GetAwaiter().GetResult();
                }

            });
        }

        private string TestAndSetEnvironmentVariable(string environmentVariable, string valueIfNull)
        {
            var value = Environment.GetEnvironmentVariable(environmentVariable);
            if(value == null)
            {
                Environment.SetEnvironmentVariable(environmentVariable, valueIfNull);
                value = valueIfNull;
            }
            return value;
        }
    }
}
