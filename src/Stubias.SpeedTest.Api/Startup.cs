using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stubias.SpeedTest.Api.Actions;
using Stubias.SpeedTest.Api.Data.Services;
using Stubias.SpeedTest.Api.Models;
using Stubias.SpeedTest.Api.Models.Input;
using Swashbuckle.AspNetCore.Swagger;

namespace Stubias.SpeedTest.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Speed Test API", Version = "v1" });
            });

            var awsOptions = Configuration.GetAWSOptions();

            if(Environment.IsDevelopment())
            {
                awsOptions.DefaultClientConfig.ServiceURL = Configuration["AWS:DynamoDbEndpoint"];
            }

            services.AddDefaultAWSOptions(awsOptions)
                .AddAWSService<IAmazonDynamoDB>(awsOptions)
                .AddScoped<DynamoDBContextConfig>(
                    _ => new DynamoDBContextConfig { TableNamePrefix = Environment.EnvironmentName }
                )
                .AddScoped<IDynamoDBContext, DynamoDBContext>()
                .AddScoped<IQueryTestResultsOperation, QueryTestResultsOperation>()
                .AddScoped<IWriteTestResultOperation, WriteTestResultOperation>()
                .AddScoped<IGetAction<SpeedTestResultInputModel, IEnumerable<SpeedTestResult>>, SpeedTestResultGetAction>()
                .AddScoped<IPostAction<SpeedTestResult, SpeedTestResult>, SpeedTestResultPostAction>()
                .AddAutoMapper();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Configuration["Swagger:DefinitionPath"], "Speed Test API");
            });
        }
    }
}
