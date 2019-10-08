using Smapiot.Billing.Domain.Models;
using System.Threading.Tasks;

namespace Smapiot.Billing.Domain.Services
{
    public interface IReportCalculatorService
    {
        Task<MonthlyReport> CalculateReport(string subscriptionId, int year, int month);
    }
}
