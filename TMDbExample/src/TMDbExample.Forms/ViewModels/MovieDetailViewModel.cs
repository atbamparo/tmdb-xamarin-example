using TMDbExample.Core.Model;

namespace TMDbExample.Forms.ViewModels
{
    public class MovieDetailViewModel
    {
        public string Title { get; set; }
        public Movie Movie { get; set; }
        public MovieDetailViewModel(Movie movie = null)
        {
            Title = movie?.Title;
            Movie = movie;
        }
    }
}
