using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stubias.SpeedTest.Api.Data.Models;
using Stubias.SpeedTest.Api.Data.Services;
using Stubias.SpeedTest.Api.Models;
using Xunit;

namespace Stubias.SpeedTest.Api.Actions
{
    public class SpeedTestResultPostActionTests
    {
        private readonly Mock<IWriteTestResultOperation> _operation;
        private readonly Mock<IMapper> _mapper;
        public SpeedTestResultPostActionTests()
        {
            _mapper = new Mock<IMapper>();
            _operation = new Mock<IWriteTestResultOperation>();
        }

        [Fact]
        public async Task SuccessfullyCreatsTestResult()
        {
            //Arrange
            var input = new SpeedTestResult
            {
                Location = "test",
                ExecutionDateTime = DateTime.Parse("2019-01-01T00:00:00.000Z")
            };
            var inputData = new SpeedTestResultDataModel
            {
                Location = "test",
                ExecutionDateTime = DateTime.Parse("2019-01-01T00:00:00.000Z")
            };

            _mapper.Setup(m => m.Map<SpeedTestResult, SpeedTestResultDataModel>(input)).Returns(inputData);
            _operation.Setup(o => o.ExecuteAsync(inputData)).Returns(Task.CompletedTask);

            var action = new SpeedTestResultPostAction(_operation.Object, _mapper.Object);

            //Act
            var result = await action.PostAsync(input);

            //Assert
            Assert.IsAssignableFrom<CreatedAtRouteResult>(result.Result);
        }
    }
}