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
    public class UpcomingMoviesViewModel : BaseViewModel
    {
        private readonly IMoviesService _moviesService;

        private bool _loaded = false;
        public bool Loaded { get => _loaded; private set => SetProperty(ref _loaded, value); }

        public ObservableCollection<Movie> Movies { get; }

        public Command LoadUpcomingMoviesCommand { get; }
        public Command LoadNextPageCommand { get; }

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
                var movies = await _moviesService.GetUpcomingMoviesPageAsync(true);
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
                var movies = await _moviesService.GetUpcomingMoviesPageAsync();
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

        private void AddMovies(IEnumerable<Movie> movies)
        {
            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }
        }
    }
}