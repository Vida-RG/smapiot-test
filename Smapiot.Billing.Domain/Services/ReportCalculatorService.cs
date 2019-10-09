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

            var requestsForSubscription = 
                RequestsOfSubscription(subscriptionId, allRequests)
                .ToArray();

            MonthlyReport result = null;
            if (requestsForSubscription.Any())
            {
                var calculatedCosts = CalculateCostPerServices(requestsForSubscription);



                result =
                    new MonthlyReport()
                    {
                        TotalNumberOfRequests = requestsForSubscription.Count(),
                        SubscriptionId = requestsForSubscription.First().SubscriptionId,
                        StartDate = new DateTime(year, month, 1),
                        EndDate =
                            new DateTime(
                                year,
                                month,
                                DateTime.DaysInMonth(year, month)),
                        Costs = calculatedCosts,
                        EstimatedForRemaining = null
                    };
            }

            return result;
        }

        public IEnumerable<Request> RequestsOfSubscription(string subscriptionId, Requests allRequests)
        {
            if (allRequests == null)
            {
                throw new ArgumentNullException(nameof(allRequests));
            }

            return allRequests.Requests1
                .Where(request => request.SubscriptionId == subscriptionId)
                .ToArray();
        }

        public IEnumerable<CostPerService> CalculateCostPerServices(IEnumerable<Request> requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }

            var result =
                requests
                    .GroupBy(request => request.ServiceName)
                    .Select(group =>
                    {
                        string serviceName = group.First().ServiceName;
                        decimal pricing = GetPricingForService(serviceName);
                        int countOfCalls = group.Count();

                        return new CostPerService()
                        {
                            ServiceName = serviceName,
                            TotalPrice = pricing * countOfCalls,
                            NumberOfRequests = countOfCalls
                        };
                    });

            return result;
        }

        public decimal GetPricingForService(string serviceName)
        {
            Random random = new Random();

            return (decimal)(1d + random.NextDouble());
        }
    }
}
