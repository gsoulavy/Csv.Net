namespace Csv.Tests.Models
{
    public class FileWithHeaderWithoutFileAttribute
    {
        [CsvColumn(Name = "name")] public string Name { get; set; }
        [CsvColumn(Name = "age", Position = 1)] public int Age { get; set; }
    }
}