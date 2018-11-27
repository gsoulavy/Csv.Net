namespace Csv {
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class CsvIgnoreAttribute : Attribute { }
}