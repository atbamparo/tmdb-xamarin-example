using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TMDbExample.Forms.Converters
{
    public class EmptyImageUrlToPlaceholder : IValueConverter
    {
        public string EmptyPlaceholderUrl { get; set; } =
            "http://www.theprintworks.com/wp-content/themes/psBella/assets/img/film-poster-placeholder.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = (string)value;
            return string.IsNullOrWhiteSpace(url)
                ? EmptyPlaceholderUrl
                : url;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = (string)value;
            return url == EmptyPlaceholderUrl
                ? null
                : url;
        }
    }
}
