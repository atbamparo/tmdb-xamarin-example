using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbExample.Core.Repository.API.Data;

namespace TMDbExample.Core.Repository.API
{
    public interface IConfigurationRepository
    {
        Task<ConfigurationData> GetConfigurationAsync();
        Task<IEnumerable<GenreData>> GetGenresAsync();
    }
}
