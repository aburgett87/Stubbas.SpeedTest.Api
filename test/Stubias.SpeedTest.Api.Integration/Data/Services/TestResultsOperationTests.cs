using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Util;
using Stubias.SpeedTest.Api.Data.Models;
using Stubias.SpeedTest.Api.Data.Services;
using Xunit;

namespace Stubias.SpeedTest.Api.Integration.Data.Services
{
    public class TestResultsOperationTests
    {

        [Fact]
        public async Task WriteAndReadTestResultsToDatabase()
        {
            //Arrange
            var location = "home";
            var executionTime = DateTime.Parse("2008-09-15T09:30:41.775Z");
            var queryStart = DateTime.Parse("2008-09-14");
            var queryEnd = DateTime.Parse("2008-09-16");
            var testResult = new SpeedTestResultDataModel
            {
                Location = location,
                ExecutionDateTime = executionTime
            };

            var config = new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" };
            var dynamoDbClient = new AmazonDynamoDBClient(config);
            var dynamoDbContext = new DynamoDBContext(dynamoDbClient);
            var writeOperation = new WriteTestResultOperation(dynamoDbContext);
            var queryOperation = new QueryTestResultsOperation(dynamoDbContext);

            //Act
            await writeOperation.ExecuteAsync(testResult);
            var actualTestResultList = await queryOperation.ExecuteAsync(location, queryStart, queryEnd);
            var actualTestResult = actualTestResultList.Single();

            //Assert
            Assert.Equal(location, actualTestResult.Location);
            Assert.Equal(executionTime, actualTestResult.ExecutionDateTime);
        }
    }
}