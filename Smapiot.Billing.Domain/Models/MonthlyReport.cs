using System;
using System.Collections.Generic;

namespace Smapiot.Billing.Domain.Models
{
    public class MonthlyReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SubscriptionId { get; set; }
        public int TotalNumberOfRequests { get; set; }
        public IEnumerable<CostPerService> Costs { get; set; }
        public IEnumerable<CostPerService> EstimatesForMonth { get; set; }
    }
}
