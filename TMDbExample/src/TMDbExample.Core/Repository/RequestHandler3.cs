using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TMDbExample.Core.Repository
{
    public class RequestHandler3 : IDisposable, IRequestHandler
    {
        private const string BaseAddress = "https://api.themoviedb.org";
        private const string ApiVersion = "3";

        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly JsonSerializer _jsonSerializer;

        public RequestHandler3(HttpClient client, string apiKey)
        {
            _client = client;
            _apiKey = apiKey;
            _jsonSerializer = CreateJsonSerializer();
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<T> SendAsync<T>(HttpMethod method, string requestUri)
        {
            var request = new HttpRequestMessage(method, CreateRequestUri(requestUri));
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await ParseResponse<T>(response);
        }

        private string CreateRequestUri(string requestUri)
        {
            var absoluteUri = $"{BaseAddress}/{ApiVersion}/{requestUri}";
            return ConcatApiKey(absoluteUri);
        }

        private string ConcatApiKey(string requestUri)
        {
            var queryJoint = !requestUri.Contains("?") ? '?' : '&';
            return $"{requestUri}{queryJoint}api_key={_apiKey}";
        }

        private async Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
            using (var file = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(file))
            using (var reader = new JsonTextReader(streamReader))
            {
                return _jsonSerializer.Deserialize<T>(reader);
            }
        }

        private JsonSerializer CreateJsonSerializer()
        {
            var serializer = JsonSerializer.CreateDefault();
            serializer.ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            return serializer;
        }
    }
}
