using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stubias.SpeedTest.Api.Actions;
using Stubias.SpeedTest.Api.Data.Services;
using Stubias.SpeedTest.Api.Models;
using Stubias.SpeedTest.Api.Models.Configuration;
using Stubias.SpeedTest.Api.Models.Input;
using Swashbuckle.AspNetCore.Swagger;

namespace Stubias.SpeedTest.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        public ILogger Logger { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment environment,
            ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            Environment = environment;
            Logger = loggerFactory.CreateLogger<Startup>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var authSection = Configuration.GetSection("Auth");
            var authConfig = authSection.Get<Auth>();
            services.Configure<Auth>(authSection);
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = authConfig.Authority;
                    options.Audience = authConfig.Audience;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Speed Test API", Version = "v1" });

                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = authConfig.AuthorizationUrl,
                    Scopes = authConfig.Scopes.ToDictionary(s => s.Name, s => s.Description)
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", authConfig.Scopes.Select(s => s.Name) }
                });
            });

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions())
                .AddAWSService<IAmazonDynamoDB>()
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
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var authConfig = Configuration.GetSection("Auth").Get<Auth>();
                c.SwaggerEndpoint(Configuration["Swagger:DefinitionPath"], "Speed Test API");
                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                {
                    { "audience", authConfig.Audience } 
                });
            });
        }
    }
}
