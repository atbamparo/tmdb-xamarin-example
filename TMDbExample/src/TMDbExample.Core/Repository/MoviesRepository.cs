using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Repository.API.Data;

namespace TMDbExample.Core.Repository
{
    public class MoviesRepository : IMoviesRepository
    {
        private IRequestHandler _handler;
        public MoviesRepository(IRequestHandler handler)
        {
            _handler = handler;
        }

        public Task<UpcomingMoviesData> GetUpcomingMoviesAsync(int page, string language, string region)
        {
            var query = CreateQueryParameters(new Dictionary<string, string>
            {
                ["page"] = page.ToString(),
                ["language"] = language,
                ["region"] = region
            });
            return _handler.SendAsync<UpcomingMoviesData>(HttpMethod.Get, $"movie/upcoming{query}");
        }
            

        private string CreateQueryParameters(Dictionary<string, string> arguments)
        {
            var query = string.Join("&", arguments
                .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
                .Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return string.IsNullOrEmpty(query)
                ? string.Empty
                : $"?{query}";
        }
    }
}
