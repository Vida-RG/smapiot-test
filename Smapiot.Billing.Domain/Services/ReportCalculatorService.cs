using System;
using System.Collections.Generic;
using System.Linq;
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

            var allRequests = await _requestCounterService._api_requests_year_month_getAsync(year, month);

            var requestsForSubscription = RequestsOfSubscription(subscriptionId, allRequests);




            throw new NotImplementedException();
        }

        public IEnumerable<Request> RequestsOfSubscription(string subscriptionId, Requests allRequests)
        {
            return allRequests.Requests1
                .Where(request => request.Id == subscriptionId)
                .ToArray();
        }
    }
}
