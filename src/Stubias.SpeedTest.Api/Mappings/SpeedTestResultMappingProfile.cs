using AutoMapper;
using Stubias.SpeedTest.Api.Data.Models;
using Stubias.SpeedTest.Api.Models;

namespace Stubias.SpeedTest.Api.Mappings
{
    public class SpeedTestResultMappingProfile : Profile
    {
        public SpeedTestResultMappingProfile()
        {
            CreateMap<SpeedTestResult, SpeedTestResultDataModel>();
            CreateMap<SpeedTestResultDataModel, SpeedTestResult>();
        }
    }
}