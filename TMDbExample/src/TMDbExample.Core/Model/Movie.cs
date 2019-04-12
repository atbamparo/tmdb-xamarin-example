using System;
using System.Collections.Generic;

namespace TMDbExample.Core.Model
{
    public class Movie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public ICollection<string> Genres { get; set; }
        public DateTime ReleaseDate { get; set; }

        public string BackdropUrl { get; set; }
        public string PosterUrl { get; set; }
    }
}