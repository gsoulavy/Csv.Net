namespace Csv.Tests.Deserialize
{
    using System.IO;
    using System.Linq;
    using Models;
    using Xunit;

    public class ResolveFileWithHeader
    {
        [Fact]
        public void FileAttributeIsDefined()
        {
            var text = File.ReadAllText(@".\Files\FileWithHeader.csv");
            var result = CsvConvert.Deserialize<FileWithHeader>(text);
            Assert.Equal(3, result.Count());
        }
    }
}