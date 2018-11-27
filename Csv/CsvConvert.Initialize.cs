namespace Csv
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static partial class CsvConvert
    {
        private static CsvFileAttribute _fileAttribute;

        private static readonly ICollection<(PropertyInfo property, CsvColumnAttribute attribute)> ColumnAttributes =
            new List<(PropertyInfo property, CsvColumnAttribute attribute)>();

        private static List<(int index, PropertyInfo propertyInfo)> _positions;

        private static void Initialize<T>()
        {
            _fileAttribute = typeof(T).GetTypeInfo().GetCustomAttribute<CsvFileAttribute>();
            if (_fileAttribute is null)
                throw new ArgumentException($"CsvFileAttribute is not defined for the {typeof(T).Name}");
            var count = 0;
            foreach (var property in typeof(T).GetProperties())
            {
                if (Attribute.GetCustomAttribute(property, typeof(CsvIgnoreAttribute)) is CsvIgnoreAttribute) continue;
                var attribute =
                    Attribute.GetCustomAttribute(property, typeof(CsvColumnAttribute)) as CsvColumnAttribute;
                ColumnAttributes.Add((property,
                    attribute ?? new CsvColumnAttribute(property.Name) {Position = count}));
                count++;
            }

            _positions = new List<(int index, PropertyInfo propertyName)>();
        }
    }
}