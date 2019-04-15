using System;
using System.Collections.Generic;
using System.Text;

namespace TMDbExample.Core.Model
{
    public class ImageConfiguration
    {
        public string BaseUrl { get; set; }

        public List<string> BackdropSizes { get; set; }

        public List<string> PosterSizes { get; set; }
    }
}
