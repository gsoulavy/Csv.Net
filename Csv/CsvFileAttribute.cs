namespace Csv {
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class CsvFileAttribute : Attribute
    {
        public bool HasHeaders { get; set; }
        public char Separator { get; set; } = ',';
        public char StringQuotes { get; set; } = '\0';
    }
}