using Microsoft.AspNetCore.Mvc;
using Smapiot.RequestCounter;
using System.Threading.Tasks;

namespace Smapiot.Billing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly RequestCounterClient _requestCounterClient;

        public ReportsController(RequestCounterClient requestCounterClient)
        {
            _requestCounterClient = requestCounterClient;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = await _requestCounterClient._api_requests_year_month_getAsync(2019, 3);

            return Ok();
        }
    }
}
