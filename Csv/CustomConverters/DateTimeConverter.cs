using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSoulavy.Csv.CustomConverters
{
    using System.Globalization;

    public static class DateTimeConverter
    {
        public static DateTime ConvertFromString(string text, string format)
        {
            return DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsedDate) ? parsedDate : DateTime.MinValue;
        }
    }
}
