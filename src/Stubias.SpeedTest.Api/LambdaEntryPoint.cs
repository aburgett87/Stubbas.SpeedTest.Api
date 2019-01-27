using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Stubias.SpeedTest.Api
{
    public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder
                .ConfigureAppConfiguration((hostingContext, config) => 
                {
                    config.AddSecretsManager();
                    config.AddSystemsManager($"/speedtest/{hostingContext.HostingEnvironment.EnvironmentName}/");
                })
                .UseStartup<Startup>();
        }
    }
}
