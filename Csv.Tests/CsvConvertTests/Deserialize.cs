namespace GSoulavy.Csv.Tests.CsvConvertTests
{
    using System;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using Models;
    using Xunit;

    public class Deserialize
    {
        [Fact]
        public void FileAttributeIsDefined()
        {
            var text = File.ReadAllText(@".\Files\FileWithHeader.csv");
            var result = CsvConvert.Deserialize<FileWithHeader>(text).ToList();
            result.Count().Should().Be(3);
            result.First().Name.Should().Be("Gabs");
        }

        [Fact]
        public void FileHeaderContainsQuotes()
        {
            var text = File.ReadAllText(@".\Files\FileWithQuotes.csv");
            var result = CsvConvert.Deserialize<FileWithQuotes>(text).ToList();
            result.First().Title.Should().Be("Greetings");
        }

        [Fact]
        public void FileWithHeaderAndDate()
        {
            var text = File.ReadAllText(@".\Files\FileWithHeaderAndDate.csv");
            var result = CsvConvert.Deserialize<FileWithHeaderAndDate>(text).ToList();
            result.First().Name.Should().Be("Gabs");
            result.First().DateOfBirth.Should().Be(new DateTime(1980, 10, 11));
        }

        [Fact]
        public void FileWithHeaderDateAndSemi()
        {
            var text = File.ReadAllText(@".\Files\FileWithHeaderDateAndSemi.csv");
            var result = CsvConvert.Deserialize<FileWithHeaderDateAndSemi>(text).ToList();
            result.First().Name.Should().Be("Gabs");
            result.First().DateOfBirth.Should().Be(new DateTime(1980, 10, 11));
        }
    }
}