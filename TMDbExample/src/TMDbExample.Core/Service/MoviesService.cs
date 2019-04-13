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

        private int _nextPage;
        private int? _totalPages;
        private bool HaveNextPage => !_totalPages.HasValue || _totalPages.Value >= _nextPage;

        public MoviesService(IConfigurationService configurationService, IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
            _configurationService = configurationService;
            ResetToFirstPage();
        }

        public async Task<Movie> GetMovieAsync(string id)
        {
            await _configurationService.ConfigureIfNeededAsync();
            var movieData = await _moviesRepository.GetMovieAsync(id);
            return MapToMovie(movieData);
        }

        public async Task<IEnumerable<Movie>> GetUpcomingMoviesPageAsync(bool fromFirstPage = false)
        {
            await _configurationService.ConfigureIfNeededAsync();

            if (fromFirstPage)
            {
                ResetToFirstPage();
            }

            if (!HaveNextPage)
            {
                return Enumerable.Empty<Movie>();
            }

            var upcoming = await _moviesRepository.GetUpcomingMoviesAsync(_nextPage);
            var movies = MapToMovies(upcoming);

            _totalPages = upcoming.TotalPages;
            _nextPage++;

            return movies;
        }

        private void ResetToFirstPage()
        {
            _nextPage = 1;
            _totalPages = null;
        }

        private IEnumerable<Movie> MapToMovies(UpcomingMoviesData movies)
        {
            var imageConfiguration = _configurationService.GetImageConfiguration();
            return movies.Results.Select(data =>
            {
                var (posterUrl, backdropUrl) = GetImageUrls(imageConfiguration, data.PosterPath, data.BackdropPath);
                var movie = new Movie
                {
                    Id = data.Id,
                    Title = data.Title,
                    OriginalTitle = data.OriginalTitle,
                    PosterUrl = posterUrl,
                    BackdropUrl = backdropUrl,
                    ReleaseDate = data.ReleaseDate,
                    Genres = data.GenreIds.Select(id => _configurationService.GetGenre(id)).ToList()
                };
                return movie;
            });
        }

        private Movie MapToMovie(MovieData data)
        {
            var imageConfiguration = _configurationService.GetImageConfiguration();
            var (posterUrl, backdropUrl) = GetImageUrls(imageConfiguration, data.PosterPath, data.BackdropPath);

            return new Movie
            {
                Id = data.Id,
                Title = data.Title,
                OriginalTitle = data.OriginalTitle,
                PosterUrl = posterUrl,
                BackdropUrl = backdropUrl,
                ReleaseDate = data.ReleaseDate,
                Genres = data.Genres.Select(g => g.Name).ToList()
            };
        }

        private (string posterUrl, string backdropUrl) GetImageUrls(ImageConfiguration config, string posterPath, string backdropPath)
        {
            string posterUrl = null;
            if (!string.IsNullOrWhiteSpace(posterPath))
            {
                var posterSize = config.PosterSizes.Count > 3 ? config.PosterSizes[3] : config.PosterSizes.LastOrDefault();
                posterUrl = $"{config.BaseUrl}{posterSize}{posterPath}";
            }

            string backdropUrl = null;
            if (!string.IsNullOrWhiteSpace(backdropPath))
            {
                var backdropSize = config.BackdropSizes.Count > 1 ? config.BackdropSizes[1] : config.BackdropSizes.LastOrDefault();
                backdropUrl = $"{config.BaseUrl}{backdropSize}{backdropPath}";
            }
            
            return (posterUrl, backdropUrl);
        }

        
    }
}
