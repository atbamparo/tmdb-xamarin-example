using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbExample.Core.Model;

namespace TMDbExample.Core.Service.API
{
    public interface IConfigurationService
    {
        Task ConfigureIfNeededAsync();

        ImageConfiguration GetImageConfiguration();

        string GetGenre(string genreId);
    }
}
