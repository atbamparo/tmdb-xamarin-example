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
    }
}