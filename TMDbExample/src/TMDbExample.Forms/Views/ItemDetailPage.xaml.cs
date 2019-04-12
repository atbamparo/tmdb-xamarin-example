using TMDbExample.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMDbExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }
    }
}