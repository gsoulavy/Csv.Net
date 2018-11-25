namespace Csv.Tests.Models
{
    [CsvFile(HasHeaders = true, Separator = ',')]
    public class FileWithHeader
    {
        [CsvColumn("name")] public string Name { get; set; }
        [CsvColumn("age")] public int Age { get; set; }
        public string Title { get; set; }
    }
}