namespace Smapiot.Billing.Domain.Models
{
    public class CostPerService
    {
        public string ServiceName { get; set; }
        public decimal TotalPrice { get; set; }
        public int NumberOfRequests { get; set; }
    }
}