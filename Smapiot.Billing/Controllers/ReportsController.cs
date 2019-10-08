using Microsoft.AspNetCore.Mvc;
using Smapiot.Billing.Domain.Models;
using Smapiot.Billing.Domain.Services;
using Smapiot.RequestCounter;
using System;
using System.Threading.Tasks;

namespace Smapiot.Billing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportCalculatorService _requestCounterService;

        public ReportsController(IReportCalculatorService requestCounterService)
        {
            _requestCounterService = requestCounterService;
        }

        [HttpGet("{subscriptionId:guid}")]
        public async Task<IActionResult> Get(string subscriptionId)
        {
            return await GetReportInResult(subscriptionId, DateTime.Now.Year, DateTime.Now.Month);
        }

        [HttpGet("{subscriptionId:guid}/{year:int}/{month:int}")]
        public async Task<IActionResult> GetSpecific(string subscriptionId, int year, int month)
        {
            return await GetReportInResult(subscriptionId, year, month);
        }

        private async Task<IActionResult> GetReportInResult(string subscriptionId, int year, int month)
        {
            MonthlyReport report = null;
            try
            {
                report = await _requestCounterService.CalculateReport(subscriptionId, DateTime.Now.Year, DateTime.Now.Month);
            }
            catch (ApiException<Error> error)
            {
                if (error.StatusCode == 400)
                {
                    return BadRequest(error.Message);
                }

                if (error.StatusCode == 404)
                {
                    return NotFound(error.Message);
                }
            }
            catch (ApiException)
            {
                throw new ApplicationException("Unexpected error orrured, please contact an administrator of the service");
            }

            return Ok(report);
        }
    }
}
