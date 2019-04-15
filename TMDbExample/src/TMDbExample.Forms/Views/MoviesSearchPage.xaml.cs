using System.Collections.Generic;
using System.Linq;
using TMDbExample.Core.Model;
using TMDbExample.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMDbExample.Forms.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MoviesSearchPage : ContentPage
	{
        private MoviesSearchViewModel _viewModel;
		public MoviesSearchPage ()
		{
            BindingContext = _viewModel = new MoviesSearchViewModel();
			InitializeComponent ();

            _viewModel.OnSearchCompletes += (_, __) => ScrollToTop();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var movies = (IList<Movie>)MoviesList.ItemsSource;
            if (movies.Count == 0)
            {
                MoviesSearch.Focus();
            }
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Movie movie))
                return;

            var movieDetailViewModel = new MovieDetailViewModel(movie);
            var movieDetailPage = new MovieDetailPage(movieDetailViewModel);
            await Navigation.PushAsync(movieDetailPage);
            ((ListView)sender).SelectedItem = null;
        }

        private void LoadMoreIfNeeded(object sender, ItemVisibilityEventArgs e)
        {
            if (!(e.Item is Movie movie) || !(sender is ListView listView))
            {
                return;
            }

            if (listView.ItemsSource is IList<Movie> items && items.Skip(items.Count - 5).Contains(movie))
            {
                _viewModel.GetNextPageCommand.Execute(null);
            }
        }

        private void ScrollToTop()
        {
            var movies = (IList<Movie>)MoviesList.ItemsSource;
            MoviesList.ScrollTo(movies.First(), ScrollToPosition.Start, false);
        }
    }
}