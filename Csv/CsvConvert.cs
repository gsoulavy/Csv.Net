namespace Csv
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public static class CsvConvert
    {
        private static CsvFileAttribute _fileAttribute;
        private static ICollection<(PropertyInfo property, CsvColumnAttribute attribute)> _columnAttributes = new List<(PropertyInfo property, CsvColumnAttribute attribute)>();

        public static T Deserialize<T>(string csv)
        {
            InitializeCustomAttributes<T>();
            var result = new List<T>();
            string[] header;
            using (var stringReader = new StringReader(csv))
            {
                var line = string.Empty;
                do
                {
                    line = stringReader.ReadLine();
                    if (line == null) throw new ArgumentException("The Csv file empty");
                    if (_fileAttribute.IncludeHeaders)
                    {
                        header = line.Split(_fileAttribute.Separator);
                        line = stringReader.ReadLine();
                    }

                    var dataLine = line?.Split(_fileAttribute.Separator) ?? throw new ArgumentException("The Csv file does not contain data");

                } while (line != null);
               
            }

            return default(T);
        }

        private static void InitializeCustomAttributes<T>()
        {
            _fileAttribute = typeof(T).GetTypeInfo().GetCustomAttribute<CsvFileAttribute>();
            if (_fileAttribute is null)
                throw new ArgumentException($"CsvFileAttribute is not defined for the {typeof(T).Name}");
            
            foreach (var property in typeof(T).GetProperties())
            {
                if (!Attribute.IsDefined(property, typeof(CsvColumnAttribute))) continue;
                var attribute = Attribute.GetCustomAttribute(property, typeof(CsvColumnAttribute)) as CsvColumnAttribute;
                _columnAttributes.Add((property, attribute));
            }
        }
    }
}