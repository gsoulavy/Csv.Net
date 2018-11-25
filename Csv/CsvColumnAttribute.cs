namespace Csv
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class CsvColumnAttribute : Attribute
    {
        public string Name { get; }
        public int Position { get; set; } = -1;

        public CsvColumnAttribute(string name)
        {
            Name = name;
        }
    }
}