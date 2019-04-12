using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbExample.Core.Model;
using TMDbExample.Core.Repository.API;
using TMDbExample.Core.Repository.API.Data;
using TMDbExample.Core.Service.API;

namespace TMDbExample.Core.Service
{
    public class MoviesService : IMoviesService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IMoviesRepository _moviesRepository;

        public MoviesService(IConfigurationService configurationService, IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
            _configurationService = configurationService;
        }

        public async Task<Movie> GetMovieAsync(string id)
        {
            await _configurationService.ConfigureIfNeededAsync();
            var movieData = await _moviesRepository.GetMovieAsync(id);
            return MapToMovie(movieData);
        }

        public async Task<IEnumerable<Movie>> GetUpcomingMoviesAsync(int page)
        {
            await _configurationService.ConfigureIfNeededAsync();
            var upcoming = await _moviesRepository.GetUpcomingMoviesAsync(page);
            return MapToMovies(upcoming);
        }

        private IEnumerable<Movie> MapToMovies(UpcomingMoviesData movies)
        {
            var imageConfiguration = _configurationService.GetImageConfiguration();
            var (posterSize, backdropSize) = GetPreferredImageSizes(imageConfiguration);

            return movies.Results.Select(data => new Movie
            {
                Id = data.Id,
                Title = data.Title,
                OriginalTitle = data.OriginalTitle,
                PosterUrl = $"{imageConfiguration.BaseUrl}/{posterSize}{data.PosterPath}",
                BackdropUrl = $"{imageConfiguration.BaseUrl}/{backdropSize}{data.PosterPath}",
                Genres = data.GenreIds.Select(id => _configurationService.GetGenre(id))
            });
        }

        private Movie MapToMovie(MovieData data)
        {
            var imageConfiguration = _configurationService.GetImageConfiguration();
            var (posterSize, backdropSize) = GetPreferredImageSizes(imageConfiguration);

            return new Movie
            {
                Id = data.Id,
                Title = data.Title,
                OriginalTitle = data.OriginalTitle,
                PosterUrl = $"{imageConfiguration.BaseUrl}/{posterSize}{data.PosterPath}",
                BackdropUrl = $"{imageConfiguration.BaseUrl}/{backdropSize}{data.PosterPath}",
                Genres = data.Genres.Select(g => g.Name)
            };
        }

        private (string posterSize, string backdropSize) GetPreferredImageSizes(ImageConfiguration config)
        {
            var posterSize = config.PosterSizes.Count > 1 ? config.PosterSizes[1] : config.PosterSizes.FirstOrDefault();
            var backdropSize = config.BackdropSizes.Count > 1 ? config.BackdropSizes[1] : config.BackdropSizes.FirstOrDefault();
            return (posterSize, backdropSize);
        }
    }
}
