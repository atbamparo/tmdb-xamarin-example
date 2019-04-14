using System.Collections.Generic;

namespace TMDbExample.Core.Repository.API.Data
{
    public class MoviesData
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public List<MovieListData> Results { get; set; }
    }
}
