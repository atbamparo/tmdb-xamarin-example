using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

        public Task<MoviesData> SearchMoviesAsync(string query, int page = 1, string language = null, string region = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("Argument is required", nameof(query));
            }

            var queryParameters = CreateQueryParameters(new Dictionary<string, string>
            {
                ["query"] = HttpUtility.UrlEncode(query),
                ["page"] = page.ToString(),
                ["language"] = language,
                ["region"] = region
            });
            return _handler.SendAsync<MoviesData>(HttpMethod.Get, $"search/movie{queryParameters}");
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
