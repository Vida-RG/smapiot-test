using Microsoft.Extensions.Options;

namespace Smapiot.RequestCounter
{
    public partial class RequestCounterClient
    {
        private readonly string _apiKey;

        public RequestCounterClient(
            System.Net.Http.HttpClient httpClient,
            IOptionsMonitor<RequestCounterApiKey> optionsMonitor)
            : this(httpClient)
        {
            _apiKey = optionsMonitor.CurrentValue.ApiKeyRequestCounter;
        }

        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder)
        {
            urlBuilder.Append($"?code={_apiKey}");
        }
    }
}
