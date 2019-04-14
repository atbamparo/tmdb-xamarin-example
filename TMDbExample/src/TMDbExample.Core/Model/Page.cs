using System;
using System.Collections.Generic;
using System.Text;

namespace TMDbExample.Core.Model
{
    public class Page<T>
    {
        public int TotalPages { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}
