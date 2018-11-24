namespace Csv
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class CsvColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public int Position { get; set; } = -1;
    }
}