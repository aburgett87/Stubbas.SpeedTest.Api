using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Stubias.SpeedTest.Api.Data.Models;

namespace Stubias.SpeedTest.Api.Integration
{
    public class SpeedTestWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var dynamoDbUrl = "http://localhost:8000";
            Environment.SetEnvironmentVariable("AWS__DynamoDbEndpoint", dynamoDbUrl);
            builder.ConfigureServices(services =>
            {
                var awsOptions = new AWSOptions();
                awsOptions.DefaultClientConfig.ServiceURL = dynamoDbUrl;
                services.AddDefaultAWSOptions(awsOptions)
                    .AddAWSService<IAmazonDynamoDB>(awsOptions)
                    .AddScoped<IDynamoDBContext, DynamoDBContext>();

                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var dynamoDb = scope.ServiceProvider.GetRequiredService<IAmazonDynamoDB>();
                    var dynamoDbContext = scope.ServiceProvider.GetRequiredService<IDynamoDBContext>();
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
                    Task.Run(() => dynamoDbContext.SaveAsync(input)).Wait();
                }
            });
        }
    }
}