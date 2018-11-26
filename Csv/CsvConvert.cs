namespace Csv
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class CsvConvert
    {
        private static CsvFileAttribute _fileAttribute;

        private static readonly ICollection<(PropertyInfo property, CsvColumnAttribute attribute)> ColumnAttributes =
            new List<(PropertyInfo property, CsvColumnAttribute attribute)>();

        private static List<(int index, PropertyInfo propertyInfo)> _positions;

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

        private static void Initialize<T>()
        {
            _fileAttribute = typeof(T).GetTypeInfo().GetCustomAttribute<CsvFileAttribute>();
            if (_fileAttribute is null)
                throw new ArgumentException($"CsvFileAttribute is not defined for the {typeof(T).Name}");
            var count = 0;
            foreach (var property in typeof(T).GetProperties())
            {
                var attribute =
                    Attribute.GetCustomAttribute(property, typeof(CsvColumnAttribute)) as CsvColumnAttribute;
                ColumnAttributes.Add((property,
                    attribute ?? new CsvColumnAttribute(property.Name){Position = count}));
                count++;
            }

            _positions = new List<(int index, PropertyInfo propertyName)>();
        }
    }
}