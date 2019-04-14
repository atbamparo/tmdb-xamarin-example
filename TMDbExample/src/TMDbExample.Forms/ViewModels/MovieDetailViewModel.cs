using TMDbExample.Core.Model;

namespace TMDbExample.Forms.ViewModels
{
    public class MovieDetailViewModel: BaseViewModel
    {
        private string _title;
        public string Title { get => _title; private set => SetProperty(ref _title, value); }

        public Movie Movie { get; set; }
        public MovieDetailViewModel(Movie movie = null)
        {
            Title = movie?.Title;
            Movie = movie;
        }
    }
}
