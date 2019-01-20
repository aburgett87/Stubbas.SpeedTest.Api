using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Util;

namespace Stubias.SpeedTest.Api.Data.Models
{
    [DynamoDBTable(DynamoDbConstants.TableName)]
    public class SpeedTestResultDataModel
    {
        [DynamoDBHashKey(DynamoDbConstants.IdAttribute)]
        public string Location { get; set; }
        [DynamoDBRangeKey(DynamoDbConstants.SortKeyAttribute)]
        public DateTime ExecutionDateTime { get; set; }

        public decimal AverageDownloadSpeed { get; set; }
        public decimal MaximumDownloadSpeed { get; set; }
        public decimal AverageUploadSpeed { get; set; }
        public decimal MaximumUploadSpeed { get; set; }
        public decimal Latency { get; set; }
        public string TestServerName { get; set; }
        public string NodeName { get; set; }
    }
}