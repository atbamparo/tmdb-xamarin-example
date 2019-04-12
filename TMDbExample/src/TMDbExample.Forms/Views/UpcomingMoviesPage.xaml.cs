using System;
using System.Collections.Generic;
using System.Linq;
using TMDbExample.Core.Model;
using TMDbExample.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMDbExample.Views
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

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            /*
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
            */
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!ViewModel.Loaded)
                ViewModel.LoadUpcomingMoviesCommand.Execute(null);
        }

        private void ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (!(e.Item is Movie movie))
            {
                return;
            }

            if (!(sender is ListView listView))
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