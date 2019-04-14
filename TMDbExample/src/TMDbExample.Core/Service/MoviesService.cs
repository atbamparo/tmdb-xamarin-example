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

        public async Task<Page<Movie>> GetUpcomingMoviesPageAsync(int page)
        {
            await _configurationService.ConfigureIfNeededAsync();
            var upcoming = await _moviesRepository.GetUpcomingMoviesAsync(page);
            return new Page<Movie>
            {
                TotalPages = upcoming.TotalPages,
                Results = MapToMovies(upcoming)
            };
        }

        public async Task<Page<Movie>> SearchMoviesAsync(string query, int pageNumber)
        {
            await _configurationService.ConfigureIfNeededAsync();
            var searchResult = await _moviesRepository.SearchMoviesAsync(query, pageNumber);
            return new Page<Movie>
            {
                TotalPages = searchResult.TotalPages,
                Results = MapToMovies(searchResult)
            };
        }

        private IEnumerable<Movie> MapToMovies(MoviesData movies)
        {
            if (movies.Results.Count == 0)
            {
                return Enumerable.Empty<Movie>();
            }

            var imageConfiguration = _configurationService.GetImageConfiguration();
            return movies.Results.Select(data =>
            {
                var (posterUrl, backdropUrl) = GetImageUrls(imageConfiguration, data.PosterPath, data.BackdropPath);
                var movie = new Movie
                {
                    Id = data.Id,
                    Title = data.Title,
                    Overview = data.Overview,
                    PosterUrl = posterUrl,
                    BackdropUrl = backdropUrl,
                    ReleaseDate = data.ReleaseDate,
                    Genres = data.GenreIds.Select(id => _configurationService.GetGenre(id)).ToList()
                };
                return movie;
            });
        }

        private (string posterUrl, string backdropUrl) GetImageUrls(ImageConfiguration config, string posterPath, string backdropPath)
        {
            const int preferredPosterSizeIndex = 3;
            const int preferredBackdropSizeIndex = 1;

            string posterUrl = null;
            if (!string.IsNullOrWhiteSpace(posterPath))
            {
                var posterSize = config.PosterSizes.Count > preferredPosterSizeIndex
                    ? config.PosterSizes[preferredPosterSizeIndex]
                    : config.PosterSizes.LastOrDefault();
                posterUrl = $"{config.BaseUrl}{posterSize}{posterPath}";
            }

            string backdropUrl = null;
            if (!string.IsNullOrWhiteSpace(backdropPath))
            {
                var backdropSize = config.BackdropSizes.Count > preferredBackdropSizeIndex
                    ? config.BackdropSizes[preferredBackdropSizeIndex]
                    : config.BackdropSizes.LastOrDefault();
                backdropUrl = $"{config.BaseUrl}{backdropSize}{backdropPath}";
            }
            
            return (posterUrl, backdropUrl);
        }
    }
}
