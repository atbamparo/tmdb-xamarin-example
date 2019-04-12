using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbExample.Core.Model;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Repository.API.Data;
using TMDbExample.Core.Service.API;

namespace TMDbExample.Core.Service
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;

        private ImagesData _imageConfigurationData;
        public Dictionary<string, string> _genres;
        private Task _configuring;

        public ConfigurationService(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public Task ConfigureIfNeededAsync()
        {
            if (_configuring == null)
            {
                _configuring = Configure();
            }
            return _configuring;
        }
        

        public ImageConfiguration GetImageConfiguration()
        {
            EnsureConfigured();
            return MapImageConfigurationData();
        }

        public string GetGenre(string genreId)
        {
            EnsureConfigured();
            _genres.TryGetValue(genreId, out var genreName);
            return genreName;
        }

        private void EnsureConfigured()
        {
            if (_configuring == null || !_configuring.IsCompleted)
            {
                throw new InvalidOperationException($"{nameof(ConfigurationService)} is unitialized, call and wait {nameof(ConfigureIfNeededAsync)} to use it properly");
            }
        }

        private ImageConfiguration MapImageConfigurationData() =>
             new ImageConfiguration
             {
                 BaseUrl = _imageConfigurationData.BaseUrl,
                 BackdropSizes = _imageConfigurationData.BackdropSizes.ToList(),
                 PosterSizes = _imageConfigurationData.PosterSizes.ToList()
             };

        private async Task Configure()
        {
            var config = await _configurationRepository.GetConfigurationAsync();
            var genres = await _configurationRepository.GetGenresAsync();
            _imageConfigurationData = config.Images;
            _genres = genres.ToDictionary(g => g.Id, g => g.Name);
        }


    }
}
