using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TMDbExample.Core.Model;
using TMDbExample.Core.Service.API;
using Xamarin.Forms;

namespace TMDbExample.Forms.ViewModels
{
    public class MoviesSearchViewModel : BaseViewModel
    {
        private readonly IMoviesService _moviesService;

        private bool _isBusy = false;
        public bool IsBusy { get => _isBusy; private set => SetProperty(ref _isBusy, value); }

        public ObservableCollection<Movie> Movies { get; }
        public Command<string> SearchCommand { get; }

        private int _totalPages = 1;
        private int _currentPage = 1;
        private string _searchTerm = string.Empty;

        public MoviesSearchViewModel()
        {
            _moviesService = ViewModelLocator.Resolve<IMoviesService>();
            Movies = new ObservableCollection<Movie>();
            SearchCommand = new Command<string>(async query => await SearchMovies(query));
        }

        public async Task SearchMovies(string query)
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                _searchTerm = query;
                _currentPage = 1;
                var result = await _moviesService.SearchMoviesAsync(_searchTerm, _currentPage);
                Movies.Clear();
                AddMovies(result.Results);
            }
            catch(Exception ex)
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
