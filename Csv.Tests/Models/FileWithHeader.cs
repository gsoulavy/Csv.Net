namespace Csv.Tests.Models
{
    [CsvFile(IncludeHeaders = true, Separator = ',')]
    public class FileWithHeader
    {
        [CsvColumn(Name = "name")] public string Name { get; set; }
        [CsvColumn(Name = "age", Position = 1)] public int Age { get; set; }
    }
}