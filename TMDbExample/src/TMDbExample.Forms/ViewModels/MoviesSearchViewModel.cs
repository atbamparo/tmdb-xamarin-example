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

        public ObservableCollection<Movie> Movies { get; }
        public Command<string> SearchCommand { get; }
        public Command GetNextPageCommand { get; }

        private string _searchTerm = string.Empty;
        private readonly Paginator<Movie> _paginator;

        public event EventHandler OnSearchCompletes;

        public MoviesSearchViewModel()
        {
            Movies = new ObservableCollection<Movie>();

            _moviesService = ViewModelLocator.Resolve<IMoviesService>();
            _paginator = new Paginator<Movie>(page => _moviesService.SearchMoviesAsync(_searchTerm, page));
            
            SearchCommand = new Command<string>(async query => await SearchMovies(query));
            GetNextPageCommand = new Command(async () => await GetNextPage());
        }

        public async Task SearchMovies(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            await DoBusyActionAsync(async () =>
            {
                _searchTerm = query;
                _paginator.ResetPages();
                var result = await _paginator.GetPageAsync();
                Movies.Clear();
                AddMovies(result);
                OnSearchCompletes(this, null);
            });
        }

        public async Task GetNextPage()
        {
            await DoBusyActionAsync(async () =>
            {
                var result = await _paginator.GetPageAsync();
                AddMovies(result);
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
