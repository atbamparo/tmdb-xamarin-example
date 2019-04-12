using System;

namespace TMDbExample.Core.Repository.API.Data
{
    public partial class DateRangeData
    {
        public DateTimeOffset Maximum { get; set; }

        public DateTimeOffset Minimum { get; set; }
    }
}
