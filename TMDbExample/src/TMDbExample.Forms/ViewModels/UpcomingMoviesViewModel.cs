using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using TMDbExample.Core.Model;
using System.Collections.Generic;
using TMDbExample.Core.Service.API;

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

        private readonly Paginator<Movie> _paginator;

        public UpcomingMoviesViewModel()
        {
            _moviesService = ViewModelLocator.Resolve<IMoviesService>();
            _paginator = new Paginator<Movie>(page => _moviesService.GetUpcomingMoviesPageAsync(page));

            Loaded = false;
            Movies = new ObservableCollection<Movie>();
            LoadUpcomingMoviesCommand = new Command(async () => await LoadFirstPageAsync());
            LoadNextPageCommand = new Command(async () => await LoadUpcomingMoviesPageAsync());
        }

        private async Task LoadFirstPageAsync()
        {
            await DoBusyActionAsync(async () =>
            {
                _paginator.ResetPages();
                var movies = await _paginator.GetPageAsync();
                Movies.Clear();
                AddMovies(movies);
                Loaded = true;
            });
        }

        private async Task LoadUpcomingMoviesPageAsync()
        {
            await DoBusyActionAsync(async () =>
            {
                var movies = await _paginator.GetPageAsync();
                AddMovies(movies);
            });
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