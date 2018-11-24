namespace Csv {
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class CsvFileAttribute : Attribute
    {
        public bool IncludeHeaders { get; set; }
        public bool IgnoreUndefinedColumns { get; set; } = true;
        public char Separator { get; set; } = ',';
    }
}