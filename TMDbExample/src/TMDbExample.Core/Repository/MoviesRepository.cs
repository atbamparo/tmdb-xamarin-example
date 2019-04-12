using System.Net.Http;
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

        public Task<MovieData> GetMovieAsync(string id, string language = "en-US") =>
            _handler.SendAsync<MovieData>(HttpMethod.Get, $"movie/{id}?language={language}");

        public Task<UpcomingMoviesData> GetUpcomingMoviesAsync(int page, string language, string region) =>
            _handler.SendAsync<UpcomingMoviesData>(HttpMethod.Get, $"movie/upcoming?page={page}&language={language}&region={region}");
    }
}
