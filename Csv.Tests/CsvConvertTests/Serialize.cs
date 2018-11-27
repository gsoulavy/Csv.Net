namespace Csv.Tests.CsvConvertTests
{
    using System.Collections.Generic;
    using Models;
    using Xunit;

    public class Serialize
    {
        [Fact]
        public void CreateFileWithHeader()
        {
            var list = new List<FileWithHeader>
            {
                new FileWithHeader {Age = 32, Name = "Joe", Title = "Mr"},
                new FileWithHeader {Age = 11, Name = "Anna", Title = "Miss"}
            };
            var result = CsvConvert.Serialize(list);

            Assert.Equal("name, age\r\nJoe, 32\r\nAnna, 11\r\n", result);
        }
    }
}