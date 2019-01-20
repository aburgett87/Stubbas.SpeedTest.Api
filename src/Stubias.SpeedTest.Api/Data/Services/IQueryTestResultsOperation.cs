using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stubias.SpeedTest.Api.Data.Models;

namespace Stubias.SpeedTest.Api.Data.Services
{
    public interface IQueryTestResultsOperation
    {
        Task<IEnumerable<SpeedTestResultDataModel>> ExecuteAsync(string location,
            DateTime startDate,
            DateTime endDate);
    }
}