using Microsoft.AspNetCore.Mvc;
using Smapiot.RequestCounter;
using System.Threading.Tasks;

namespace Smapiot.Billing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IRequestCounterService _requestCounterClient;

        public ReportsController(IRequestCounterService requestCounterClient)
        {
            _requestCounterClient = requestCounterClient;
        }

        [HttpGet("{subscriptionId}")]
        public async Task<IActionResult> Get(string subscriptionId)
        {
            var request = await _requestCounterClient._api_requests_year_month_getAsync(2019, 3);

            return Ok();
        }

        [HttpGet("{subscriptionId}/{year}/{month}")]
        public async Task<IActionResult> GetSpecific(string subscriptionId)
        {
            var request = await _requestCounterClient._api_requests_year_month_getAsync(2019, 3);

            return Ok();
        }
    }
}
