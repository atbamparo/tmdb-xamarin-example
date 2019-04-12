using System.Collections.Generic;

namespace TMDbExample.Core.Model
{
    public class Movie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public IEnumerable<string> Genres { get; set; }

        public string BackdropUrl { get; set; }
        public string PosterUrl { get; set; }
    }
}