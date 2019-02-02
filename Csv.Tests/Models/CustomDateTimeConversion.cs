namespace GSoulavy.Csv.Tests.Models {
    using System;
    using System.Globalization;
    using Files;

    public class CustomDateTimeConversion : ICustomConversion
    {
        private const string Format = "d.M.yyyy";

        public object Parse(string value)
        {
            return DateTime.TryParseExact(value, Format, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsedDate) ? parsedDate : DateTime.MinValue;
        }

        public string Compose(object value)
        {
            return ((DateTime) value).ToString(Format);
        }
    }
}