using System.Threading.Tasks;
using TMDbExample.Core.Repository.API.Data;

namespace TMDbExample.Core.Repository.API
{
    public interface IMoviesRepository
    {
        Task<MovieData> GetMovieAsync(string id, string language = "en-US");

        Task<UpcomingMoviesData> GetUpcomingMoviesAsync(int page = 1, string language = "en-US", string region = "US");
    }
}
