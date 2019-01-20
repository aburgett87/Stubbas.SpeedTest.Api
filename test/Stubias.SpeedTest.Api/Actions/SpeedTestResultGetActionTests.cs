using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Stubias.SpeedTest.Api.Data.Models;
using Stubias.SpeedTest.Api.Data.Services;
using Stubias.SpeedTest.Api.Models;
using Stubias.SpeedTest.Api.Models.Input;
using Xunit;

namespace Stubias.SpeedTest.Api.Actions
{
    public class SpeedTestResultGetActionTests
    {
        private readonly Mock<IQueryTestResultsOperation> _operation;
        private readonly Mock<IMapper> _mapper;
        public SpeedTestResultGetActionTests()
        {
            _mapper = new Mock<IMapper>();
            _operation = new Mock<IQueryTestResultsOperation>();
        }

        [Fact]
        public async Task ReturnsTestsForDateRange()
        {
            //Arrange
            var input = new SpeedTestResultInputModel
            {
                Location = "Not Found",
                StartDateTime = DateTime.Parse("2019-01-01T00:00:00.000Z"),
                EndDateTime = DateTime.Parse("2019-01-02T00:00:00.000Z")
            };
            var resultData = new [] { new SpeedTestResultDataModel() };
            var result = new [] { new SpeedTestResult() };

            _operation.Setup(o => o.ExecuteAsync(input.Location, input.StartDateTime, input.EndDateTime))
                .ReturnsAsync(resultData);
            _mapper.Setup(m => m.Map<IEnumerable<SpeedTestResultDataModel>, IEnumerable<SpeedTestResult>>(resultData))
                .Returns(result);

            var action = new SpeedTestResultGetAction(_operation.Object, _mapper.Object);

            //Act
            var actionResult = await action.GetAsync(input);

            //Assert
            Assert.Equal(result, actionResult.Value);
        }
    }
}