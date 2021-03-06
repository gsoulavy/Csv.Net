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
            var text = File.ReadAllText(Path.Combine("Files", "FileWithHeader.csv"));
            var result = CsvConvert.Deserialize<FileWithHeader>(text).ToList();
            result.Count().Should().Be(3);
            result.First().Name.Should().Be("Gabs");
        }

        [Fact]
        public void FileHeaderContainsQuotes()
        {
            var text = File.ReadAllText(Path.Combine("Files","FileWithQuotes.csv"));
            var result = CsvConvert.Deserialize<FileWithQuotes>(text).ToList();
            result.First().Title.Should().Be("Greetings");
            result.First(r => r.Title.Contains("New York")).Title.Should().Be("New York, New York");
        }

        [Fact]
        public void FileWithHeaderAndDate()
        {
            var text = File.ReadAllText(Path.Combine("Files","FileWithHeaderAndDate.csv"));
            var result = CsvConvert.Deserialize<FileWithHeaderAndDate>(text).ToList();
            result.First().Name.Should().Be("Gabs");
            result.First().DateOfBirth.Should().Be(new DateTime(1980, 10, 11));
        }

        [Fact]
        public void FileWithHeaderAndDateAsStream()
        {
            var text = File.ReadAllText(Path.Combine("Files","FileWithHeaderAndDate.csv"));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            var result = CsvConvert.Deserialize<FileWithHeaderAndDate>(stream).ToList();
            result.First().Name.Should().Be("Gabs");
            result.First().DateOfBirth.Should().Be(new DateTime(1980, 10, 11));
        }

        [Fact]
        public void FileWithHeaderAndModelWithoutAttributes()
        {
            var text = File.ReadAllText(Path.Combine("Files","FileWithHeaderWithoutAttributes.csv"));
            var result = CsvConvert.Deserialize<FileWithHeaderWithoutAttributes>(text).ToList();
            result.Count().Should().Be(3);
            result.First().Name.Should().Be("Gabs");
            result.First().Age.Should().Be(35);
            result.Last().Name.Should().Be("Anna");
            result.Last().Age.Should().Be(23);
        }

        [Fact]
        public void FileWithHeaderDateAndSemi()
        {
            var text = File.ReadAllText(Path.Combine("Files", "FileWithHeaderDateAndSemi.csv"));
            var result = CsvConvert.Deserialize<FileWithHeaderDateAndSemi>(text).ToList();
            result.First().Name.Should().Be("Gabs");
            result.First().DateOfBirth.Should().Be(new DateTime(1980, 10, 11));
        }
    }
}
