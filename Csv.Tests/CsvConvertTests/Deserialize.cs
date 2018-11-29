namespace GSoulavy.Csv.Tests.CsvConvertTests
{
    using System.IO;
    using System.Linq;
    using Models;
    using Xunit;

    public class Deserialize
    {
        [Fact]
        public void FileAttributeIsDefined()
        {
            var text = File.ReadAllText(@".\Files\FileWithHeader.csv");
            var result = CsvConvert.Deserialize<FileWithHeader>(text).ToList();
            Assert.Equal(3, result.Count());
            Assert.Equal("Gabs", result.First().Name);
        }

        [Fact]
        public void FileHeaderContainsQuotes()
        {
            var text = File.ReadAllText(@".\Files\FileWithQuotes.csv");
            var result = CsvConvert.Deserialize<FileWithQuotes>(text).ToList();
            Assert.Equal("Greetings", result.First().Title);
        }
    }
}