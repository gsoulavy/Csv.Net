namespace GSoulavy.Csv
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
                             new CsvFileAttribute {HasHeaders = true, Separator = ','};
            _columnAttributeMappings = GetColumnAttributeMappings();
            _positions = new List<(int index, PropertyInfo propertyName)>();
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

        public IEnumerable<T> Deserialize(Stream csvStream)
        {
            using (var streamReader = new StreamReader(csvStream))
            {
                var line = streamReader.ReadLine();

                if (_fileAttribute.HasHeaders) ExtractPositions(line);

                while (!streamReader.EndOfStream) yield return ParseData(line);
            }
        }

        public string Serialize(IEnumerable<T> list)
        {
            if (!_fileAttribute.HasHeaders) return string.Empty;
            ExtractPosition();
            var header = string.Join($"{_fileAttribute.Separator} ",
                _columnAttributeMappings.OrderBy(c => c.attribute.Position).Select(c => c.attribute.Name));
            return
                $"{header}{Environment.NewLine}{ToCsvString(list)}";
        }

        internal static Serializer<T> Create()
        {
            return new Serializer<T>();
        }


        private void ExtractPosition()
        {
            foreach (var (property, attribute) in _columnAttributeMappings.OrderBy(c => c.attribute.Position))
                _positions.Add((string.Join($"{_fileAttribute.Separator} ",
                        _columnAttributeMappings.OrderBy(c => c.attribute.Position).Select(c => c.attribute.Name))
                    .IndexOf(attribute.Name, StringComparison.Ordinal), property));
        }

        private void ExtractPositions(string line)
        {
            var header = line.Split(_fileAttribute.Separator).Select(Sanitize()).ToList();
            foreach (var (property, attribute) in _columnAttributeMappings.OrderBy(c => c.attribute.Position))
                _positions.Add((header.IndexOf(attribute.Name), property));
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

        private Type GetCustomConverter(PropertyInfo propertyInfo)
        {
            return _columnAttributeMappings.FirstOrDefault(cam => cam.property == propertyInfo)
                .attribute.ColumnConverter;
        }

        private static CsvFileAttribute GetFileAttribute()
        {
            return typeof(T).GetTypeInfo().GetCustomAttribute<CsvFileAttribute>();
        }

        private T ParseData(string line)
        {
            var dataLine = line?.Split(_fileAttribute.Separator).Select(Sanitize()).ToList() ??
                           throw new ArgumentException("The Csv file does not contain data");
            var type = typeof(T);
            var instance = (T) Activator.CreateInstance(type);
            foreach (var propertyInfo in type.GetProperties())
                try
                {
                    var data = dataLine[_positions.First(c => c.propertyInfo.Name == propertyInfo.Name).index];
                    var customConverter = GetCustomConverter(propertyInfo);
                    if (customConverter is null)
                    {
                        propertyInfo.SetValue(instance,
                            TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertFrom(data));
                    }
                    else
                    {
                        var converter = (ICustomConversion) Activator.CreateInstance(customConverter);
                        propertyInfo.SetValue(instance,
                            converter.Parse(data));
                    }
                }
                catch
                {
                    // The actual value is not matched, therefore it will be ignored
                }

            return instance;
        }

        private Func<string, string> Sanitize()
        {
            return s => s.Trim().Trim(_fileAttribute.StringQuotes);
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
                    .OrderBy(j => j.Position).Select(p =>
                    {
                        var customConverter = GetCustomConverter(p.p);
                        if (customConverter is null) return p.p.GetValue(item).ToString();

                        var converter = (ICustomConversion) Activator.CreateInstance(customConverter);
                        return converter.Compose(p.p.GetValue(item));
                    });
                sb.Append(
                    $"{string.Join($"{_fileAttribute.Separator} ", data)}{Environment.NewLine}");
            }

            return sb.ToString();
        }
    }
}