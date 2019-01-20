using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Stubias.SpeedTest.Api.Data.Models;

namespace Stubias.SpeedTest.Api.Data.Services
{
    public class QueryTestResultsOperation : IQueryTestResultsOperation
    {
        private readonly IDynamoDBContext _context;
        public QueryTestResultsOperation(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SpeedTestResultDataModel>> ExecuteAsync(string location,
            DateTime startDate,
            DateTime endDate)
        {
            var results = new List<SpeedTestResultDataModel>();
            var queryOperator = QueryOperator.Between;
            var scanKeyValues = new List<object> { startDate, endDate };

            var asyncSearch = _context.QueryAsync<SpeedTestResultDataModel>(location, queryOperator, scanKeyValues);

            do
            {
                results.AddRange(await asyncSearch.GetNextSetAsync());
            } while(!asyncSearch.IsDone);

            return results;
        }
    }
}