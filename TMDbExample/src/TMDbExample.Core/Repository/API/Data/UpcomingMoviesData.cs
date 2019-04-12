using System;
using System.Collections.Generic;
using System.Text;

namespace TMDbExample.Core.Repository.API.Data
{
    public class UpcomingMoviesData
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public DateRangeData Dates { get; set; }
        public List<MovieListData> Results { get; set; }
    }
}
