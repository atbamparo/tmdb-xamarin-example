using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using TMDbExample.Core.Model;
using System.Collections.Generic;
using TMDbExample.Core.Service.API;
using System.Linq;

namespace TMDbExample.Forms.ViewModels
{
    public class UpcomingMoviesViewModel
    {
        private readonly IMoviesService _moviesService;

        public bool IsBusy { get; private set; } = false;
        public bool Loaded { get; private set; } = false;

        public ObservableCollection<Movie> Movies { get; }

        public Command LoadUpcomingMoviesCommand { get; }
        public Command LoadNextPageCommand { get; }

        private int _totalPages = 1;
        private int _currentPage;

        public UpcomingMoviesViewModel()
        {
            _moviesService = ViewModelLocator.Resolve<IMoviesService>();

            Loaded = false;
            Movies = new ObservableCollection<Movie>();
            LoadUpcomingMoviesCommand = new Command(async () => await LoadFirstPageAsync());
            LoadNextPageCommand = new Command(async () => await LoadUpcomingMoviesPageAsync());
        }

        private async Task LoadFirstPageAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                _currentPage = 1;
                var movies = await GetMoviePage();
                Movies.Clear();
                AddMovies(movies);
                Loaded = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            
        }

        private async Task LoadUpcomingMoviesPageAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var movies = await GetMoviePage();
                AddMovies(movies);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<IEnumerable<Movie>> GetMoviePage()
        {
            if (_currentPage > _totalPages)
            {
                return Enumerable.Empty<Movie>();
            }

            var moviePage = await _moviesService.GetUpcomingMoviesPageAsync(_currentPage);
            _currentPage++;
            _totalPages = moviePage.TotalPages;
            return moviePage.Results;
        }

        private void AddMovies(IEnumerable<Movie> movies)
        {
            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }
        }
    }
}