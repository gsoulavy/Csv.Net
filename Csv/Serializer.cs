namespace Csv
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal class Serializer<T> where T : class
    {
        private readonly List<(PropertyInfo property, CsvColumnAttribute attribute)> _columnAttributeMappings;
        private readonly CsvFileAttribute _fileAttribute;
        private readonly List<(int index, PropertyInfo propertyInfo)> _positions;

        public Serializer()
        {
            _fileAttribute = GetFileAttribute() ??
                             throw new ArgumentException($"CsvFileAttribute is not defined for the {typeof(T).Name}");
            ;
            _columnAttributeMappings = GetColumnAttributeMappings();
            _positions = new List<(int index, PropertyInfo propertyName)>();
        }

        private static CsvFileAttribute GetFileAttribute()
        {
            return typeof(T).GetTypeInfo().GetCustomAttribute<CsvFileAttribute>();
        }

        private static List<(PropertyInfo property, CsvColumnAttribute attribute)> GetColumnAttributeMappings()
        {
            var columnAttribute = new List<(PropertyInfo property, CsvColumnAttribute attribute)>();
            var count = 0;
            foreach (var property in typeof(T).GetProperties())
            {
                if (Attribute.GetCustomAttribute(property, typeof(CsvIgnoreAttribute)) is CsvIgnoreAttribute) continue;
                var attribute =
                    Attribute.GetCustomAttribute(property, typeof(CsvColumnAttribute)) as CsvColumnAttribute;
                columnAttribute.Add((property,
                    attribute ?? new CsvColumnAttribute(property.Name) {Position = count}));
                count++;
            }

            return columnAttribute;
        }

        internal static Serializer<T> Create()
        {
            return new Serializer<T>();
        }

        public IEnumerable<T> Deserialize(string csv)
        {
            using (var stringReader = new StringReader(csv))
            {
                var line = stringReader.ReadLine();

                if (_fileAttribute.HasHeaders) ExtractPositions(line);

                while ((line = stringReader.ReadLine()) != null) yield return ParseData(line);
            }
        }

        private T ParseData(string line)
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

        private void ExtractPositions(string line)
        {
            var header = line.Split(_fileAttribute.Separator).Select(Sanitize()).ToList();
            foreach (var (property, attribute) in _columnAttributeMappings.OrderBy(c => c.attribute.Position))
                _positions.Add((header.IndexOf(attribute.Name), property));
        }

        private Func<string, string> Sanitize()
        {
            return s => s.Trim().Trim(_fileAttribute.StringQuotes);
        }

        public string Serialize(IEnumerable<T> list)
        {
            if (_fileAttribute.HasHeaders)
            {
                ExtractPosition();
                var header = string.Join($"{_fileAttribute.Separator} ",
                    _columnAttributeMappings.OrderBy(c => c.attribute.Position).Select(c => c.attribute.Name));
                return
                    $"{header}{Environment.NewLine}{ToCsvString(list)}";
            }

            return string.Empty;
        }


        private void ExtractPosition()
        {
            foreach (var (property, attribute) in _columnAttributeMappings.OrderBy(c => c.attribute.Position))
                _positions.Add((string.Join($"{_fileAttribute.Separator} ",
                        _columnAttributeMappings.OrderBy(c => c.attribute.Position).Select(c => c.attribute.Name))
                    .IndexOf(attribute.Name, StringComparison.Ordinal), property));
        }

        private string ToCsvString(IEnumerable<T> list)
        {
            var sb = new StringBuilder();
            foreach (var item in list)
            {
                var properties = item.GetType().GetProperties();
                var data = properties
                    .Join(_columnAttributeMappings, p => p.Name, c => c.property.Name,
                        (p, c) => new {p, c.attribute.Position})
                    .OrderBy(j => j.Position).Select(p => p.p.GetValue(item).ToString());
                sb.Append(
                    $"{string.Join(", ", data)}{Environment.NewLine}");
            }

            return sb.ToString();
        }
    }
}