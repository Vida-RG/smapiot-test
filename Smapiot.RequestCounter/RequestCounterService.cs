namespace Smapiot.RequestCounter
{
    public class RequestCounterService : IRequestCounterService
    {
        public readonly RequestCounterClient _client;

        public RequestCounterService(RequestCounterClient client)
        {
            _client = client;
        }

        public async System.Threading.Tasks.Task<Requests> _api_requests_year_month_getAsync(int year, int month)
        {
            return await _client._api_requests_year_month_getAsync(year, month);
        }
    }
}
