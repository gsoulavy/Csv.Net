namespace GSoulavy.Csv.Tests.Models
{
    [CsvFile(HasHeaders = true, Separator = ',')]
    public class FileWithHeader
    {
        [CsvColumn("name")]
        public string Name { get; set; }

        [CsvColumn("age")]
        public int Age { get; set; }

        [CsvIgnore]
        public string Title { get; set; }
    }
}