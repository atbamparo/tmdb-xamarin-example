using System.Threading.Tasks;
using TMDbExample.Core.Repository.API.Data;

namespace TMDbExample.Core.Repository.API
{
    public interface IMoviesRepository
    {
        Task<UpcomingMoviesData> GetUpcomingMoviesAsync(int page = 1, string language = null, string region = null);
    }
}
