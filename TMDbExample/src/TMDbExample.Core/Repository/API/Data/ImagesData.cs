using System.Collections.Generic;

namespace TMDbExample.Core.Repository.API.Data
{
    public class ImagesData
    {
        public string BaseUrl { get; set; }

        public string SecureBaseUrl { get; set; }

        public List<string> BackdropSizes { get; set; }

        public List<string> LogoSizes { get; set; }

        public List<string> PosterSizes { get; set; }

        public List<string> ProfileSizes { get; set; }

        public List<string> StillSizes { get; set; }
    }
}
