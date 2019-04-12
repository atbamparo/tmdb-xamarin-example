using System;
using System.Collections.Generic;
using System.Text;

namespace TMDbExample.Core.Repository.API.Data
{
    public class ConfigurationData
    {
        public ImagesData Images { get; set; }

        public List<string> ChangeKeys { get; set; }
    }
}
