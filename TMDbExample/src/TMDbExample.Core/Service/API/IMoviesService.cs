using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbExample.Core.Model;

namespace TMDbExample.Core.Service.API
{
    public interface IMoviesService
    {
        Task<Page<Movie>> GetUpcomingMoviesPageAsync(int pageNumber);
    }
}
