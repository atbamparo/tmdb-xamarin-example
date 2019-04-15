using System;
using System.Collections.Generic;

namespace TMDbExample.Core.Repository.API.Data
{
    public partial class MovieListData
    {
        public string Id { get; set; }

        public bool Adult { get; set; }

        public string PosterPath { get; set; }

        public string BackdropPath { get; set; }

        public string Overview { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public List<string> GenreIds { get; set; }

        public string OriginalTitle { get; set; }

        public string OriginalLanguage { get; set; }

        public string Title { get; set; }

        public double Popularity { get; set; }

        public long VoteCount { get; set; }

        public bool Video { get; set; }

        public double VoteAverage { get; set; }
    }
}
