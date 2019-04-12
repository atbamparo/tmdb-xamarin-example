using TMDbExample.Core.Model;

namespace TMDbExample.Forms.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Movie Movie { get; set; }
        public ItemDetailViewModel(Movie movie = null)
        {
            Title = movie?.Title;
            Movie = movie;
        }
    }
}
