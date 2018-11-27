namespace Csv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static partial class CsvConvert
    {
        public static string Serialize<T>(IEnumerable<T> list)
        {
            Initialize<T>();
            if (_fileAttribute.HasHeaders)
            {
                ExtractPosition();
                var header = string.Join($"{_fileAttribute.Separator} ",
                    ColumnAttributes.OrderBy(c => c.attribute.Position).Select(c => c.attribute.Name));
                return
                    $"{header}{Environment.NewLine}{ToCsvString(list)}";
            }

            return string.Empty;
        }

        private static void ExtractPosition()
        {
            foreach (var (property, attribute) in ColumnAttributes.OrderBy(c => c.attribute.Position))
                _positions.Add((string.Join($"{_fileAttribute.Separator} ",
                        ColumnAttributes.OrderBy(c => c.attribute.Position).Select(c => c.attribute.Name))
                    .IndexOf(attribute.Name, StringComparison.Ordinal), property));
        }

        private static string ToCsvString<T>(IEnumerable<T> list)
        {
            var sb = new StringBuilder();
            foreach (var item in list)
            {
                var properties = item.GetType().GetProperties();
                var data = properties
                    .Join(ColumnAttributes, p => p.Name, c => c.property.Name, (p, c) => new {p, c.attribute.Position})
                    .OrderBy(j => j.Position).Select(p => p.p.GetValue(item).ToString());
                sb.Append(
                    $"{string.Join(", ", data)}{Environment.NewLine}");
            }

            return sb.ToString();
        }
    }
}