using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stubias.SpeedTest.Api.Actions
{
    public interface IPostAction<in TInput, TOutput> where TOutput: class
    {
        Task<ActionResult<TOutput>> PostAsync(TInput input);
    }
}