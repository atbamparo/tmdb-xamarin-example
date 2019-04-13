﻿using TMDbExample.Core.Model;

namespace TMDbExample.Forms.ViewModels
{
    public class MovieDetailViewModel : BaseViewModel
    {
        public Movie Movie { get; set; }
        public MovieDetailViewModel(Movie movie = null)
        {
            Title = movie?.Title;
            Movie = movie;
        }
    }
}
