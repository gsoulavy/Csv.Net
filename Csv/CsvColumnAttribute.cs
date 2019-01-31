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

        public string Name { get; }
        public int Position { get; set; } = -1;
        public string Format { get; set; } = string.Empty;
    }
}