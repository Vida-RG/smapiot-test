using System;
using System.Threading.Tasks;
using Smapiot.Billing.Domain.Models;
using Smapiot.RequestCounter;

namespace Smapiot.Billing.Domain.Services
{
    public class ReportCalculatorService : IReportCalculatorService
    {
        private readonly IRequestCounterService _requestCounterService;

        public ReportCalculatorService(IRequestCounterService requestCounterService)
        {
            _requestCounterService = requestCounterService;
        }

        public async Task<MonthlyReport> CalculateReport(string subscriptionId, int year, int month)
        {


            if (DateTime.Now.Year < year
                || (DateTime.Now.Year == year && DateTime.Now.Month < month))
            {
                return null;
            }

            var requests = await _requestCounterService._api_requests_year_month_getAsync(year, month);


            throw new NotImplementedException();
        }
    }
}
