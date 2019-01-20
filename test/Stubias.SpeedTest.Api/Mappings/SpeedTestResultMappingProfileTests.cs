using AutoMapper;
using Xunit;

namespace Stubias.SpeedTest.Api.Mappings
{
    public class SpeedTestResultMappingProfileTests
    {
        [Fact]
        public void TestMappings()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SpeedTestResultMappingProfile>();
            });

            mapperConfig.AssertConfigurationIsValid();
        }
    }
}