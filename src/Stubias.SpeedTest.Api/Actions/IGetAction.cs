using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stubias.SpeedTest.Api.Actions
{
    public interface IGetAction<in TInput, TOutput> where TOutput: class
    {
        Task<ActionResult<TOutput>> GetAsync(TInput input);
    }
}