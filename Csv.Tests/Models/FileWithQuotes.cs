namespace Csv.Tests.Models
{
    [CsvFile(HasHeaders = true, Separator = ',', StringQuotes = '"')]
    public class FileWithQuotes
    {
        public int Year { get; set; }
        public int Score { get; set; }
        public string Title { get; set; }
    }
}