using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbExample.Core.Model;

namespace TMDbExample.Core.Service.API
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetUpcomingMoviesPageAsync(bool reset = false);
    }
}
