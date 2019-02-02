namespace GSoulavy.Csv
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class CsvColumnAttribute : Attribute
    {
        public CsvColumnAttribute(string name)
        {
            Name = name;
        }

        public CsvColumnAttribute(string name, Type converter)
        {
            Name = name;
            ColumnConverter = converter;
        }

        public string Name { get; }
        public Type ColumnConverter { get; set; }
        public int Position { get; set; } = -1;
    }
}