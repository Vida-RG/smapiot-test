namespace Smapiot.RequestCounter
{
    public interface IRequestCounterService
    {
        System.Threading.Tasks.Task<Requests> _api_requests_year_month_getAsync(int year, int month);
    }
}
