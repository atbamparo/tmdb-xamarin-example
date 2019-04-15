using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Repository.API.Data;

namespace TMDbExample.Core.Repository
{
    public class ConfigurationRepository: IConfigurationRepository
    {
        private readonly IRequestHandler _handler;

        public ConfigurationRepository(IRequestHandler handler)
        {
            _handler = handler;
        }

        public Task<ConfigurationData> GetConfigurationAsync()
        {
            return _handler.SendAsync<ConfigurationData>(HttpMethod.Get, "configuration");
        }

        public async Task<IEnumerable<GenreData>> GetGenresAsync()
        {
            var genresData = await _handler.SendAsync<GenresData>(HttpMethod.Get, "genre/movie/list");
            return genresData.Genres;
        }
    }
}
