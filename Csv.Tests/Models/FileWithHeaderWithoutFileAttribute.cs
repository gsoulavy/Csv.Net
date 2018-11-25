namespace Csv.Tests.Models
{
    public class FileWithHeaderWithoutFileAttribute
    {
        [CsvColumn("name")] public string Name { get; set; }
        [CsvColumn("age")] public int Age { get; set; }
    }
}