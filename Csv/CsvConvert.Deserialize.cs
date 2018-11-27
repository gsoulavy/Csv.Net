namespace Csv
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    public static partial class CsvConvert
    {
        public static IEnumerable<T> Deserialize<T>(string csv)
        {
            Initialize<T>();

            using (var stringReader = new StringReader(csv))
            {
                var line = stringReader.ReadLine();

                if (_fileAttribute.HasHeaders) ExtractPositions(line);

                while ((line = stringReader.ReadLine()) != null) yield return ParseData<T>(line);
            }
        }

        private static T ParseData<T>(string line)
        {
            var dataLine = line?.Split(_fileAttribute.Separator).Select(Sanitize()).ToList() ??
                           throw new ArgumentException("The Csv file does not contain data");
            var type = typeof(T);
            var result = (T) Activator.CreateInstance(type);
            foreach (var propertyInfo in type.GetProperties())
                try
                {
                    var data = dataLine[_positions.First(c => c.propertyInfo.Name == propertyInfo.Name).index];
                    propertyInfo.SetValue(result,
                        TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertFrom(data));
                }
                catch
                {
                    // The actual value is not matched, therefore it will be ignored
                }

            return result;
        }

        private static void ExtractPositions(string line)
        {
            var header = line.Split(_fileAttribute.Separator).Select(Sanitize()).ToList();
            foreach (var (property, attribute) in ColumnAttributes.OrderBy(c => c.attribute.Position))
                _positions.Add((header.IndexOf(attribute.Name), property));
        }

        private static Func<string, string> Sanitize()
        {
            return s => s.Trim().Trim(_fileAttribute.StringQuotes);
        }
    }
}