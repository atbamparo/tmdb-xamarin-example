using System;
using System.Collections.Generic;
using System.Linq;
using TMDbExample.Core.Model;
using TMDbExample.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMDbExample.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpcomingMoviesPage : ContentPage
    {
        UpcomingMoviesViewModel ViewModel;

        public UpcomingMoviesPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new UpcomingMoviesViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Movie movie))
                return;

            var movieDetailViewModel = new MovieDetailViewModel(movie);
            var movieDetailPage = new MovieDetailPage(movieDetailViewModel);
            await Navigation.PushAsync(movieDetailPage);
            ItemsListView.SelectedItem = null;
        }

        private bool _navigatingToSearchPage = false;
        async void OpenSearchPage(object sender, ClickedEventArgs args)
        {
            if (_navigatingToSearchPage)
                return;

            try
            {
                _navigatingToSearchPage = true;
                var searchPage = new MoviesSearchPage();
                await Navigation.PushAsync(searchPage);
            }
            finally
            {
                _navigatingToSearchPage = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!ViewModel.Loaded)
            {
                ViewModel.LoadUpcomingMoviesCommand.Execute(null);
            }
        }

        private void ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (!(e.Item is Movie movie) || !(sender is ListView listView))
            {
                return;
            }

            if (listView.ItemsSource is IList<Movie> items && items.Skip(items.Count - 5).Contains(movie))
            {
                ViewModel.LoadNextPageCommand.Execute(null);
            }
        }
    }
}