namespace Csv.Tests.Deserialize
{
    using System.IO;
    using Models;
    using Xunit;

    public class ResolveFileWithHeader
    {
        [Fact]
        public void FileAttributeIsDefined()
        {
            var text = File.ReadAllText(@".\Files\FileWithHeader.csv");
            var result = CsvConvert.Deserialize<FileWithHeader>(text);
            Assert.NotNull(text);
        }
    }
}