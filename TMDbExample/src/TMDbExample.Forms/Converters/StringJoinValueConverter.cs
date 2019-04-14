using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace TMDbExample.Forms.Converters
{
    public class StringJoinValueConverter : IValueConverter
    {
        public string Separator { get; set; } = ", ";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var values = (IEnumerable<string>)value;

            if (parameter != null && !values.Any())
            {
                return parameter;
            }

            return string.Join(Separator, values);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).Split(new string [] { Separator }, StringSplitOptions.None).ToList();
        }
    }
}
