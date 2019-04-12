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
        private int _nextPage;
        private bool _haveNextPage;

        public bool Loaded { get; private set; }
        public ObservableCollection<Movie> Movies { get; }

        public Command LoadUpcomingMoviesCommand { get; }
        public Command LoadNextPageCommand { get; }

        public UpcomingMoviesViewModel()
        {
            _moviesService = ViewModelLocator.Resolve<IMoviesService>();

            Loaded = false;
            Movies = new ObservableCollection<Movie>();
            LoadUpcomingMoviesCommand = new Command(async () => await LoadUpcomingMoviesFirstPage());
            LoadNextPageCommand = new Command(async () => await LoadUpcomingMoviesPageAsync());
        }

        private async Task LoadUpcomingMoviesFirstPage()
        {
            if (IsBusy)
                return;

            _nextPage = 1;
            _haveNextPage = true;
            await LoadUpcomingMoviesPageAsync();
        }

        private async Task LoadUpcomingMoviesPageAsync()
        {
            if (IsBusy)
                return;

            if (!_haveNextPage)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var movies = await _moviesService.GetUpcomingMoviesAsync(_nextPage);
                _nextPage += 1;

                var isLastPage = movies.Count() == 0; // TODO: Improve by making the service layer return the total number of pages
                if (isLastPage)
                {
                    _haveNextPage = false;
                }

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