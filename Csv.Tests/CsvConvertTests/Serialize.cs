namespace GSoulavy.Csv.Tests.CsvConvertTests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
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
            result.Should().Be("name, age\r\nJoe, 32\r\nAnna, 11\r\n");
        }

        [Fact]
        public void CreateFileWithHeaderAndDate()
        {
            var list = new List<FileWithHeaderAndDate>
            {
                new FileWithHeaderAndDate
                    {Age = 32, Name = "Joe", Title = "Mr", DateOfBirth = new DateTime(1980, 10, 11)}
            };
            var result = CsvConvert.Serialize(list);
            result.Should().Be("name, age, dob\r\nJoe, 32, 11.10.1980\r\n");
        }

        [Fact]
        public void CreateFileWithHeaderDateAndSemi()
        {
            var list = new List<FileWithHeaderDateAndSemi>
            {
                new FileWithHeaderDateAndSemi
                    {Age = 32, Name = "Joe", Title = "Mr", DateOfBirth = new DateTime(1980, 10, 31)}
            };
            var result = CsvConvert.Serialize(list);
            result.Should().Be("name; age; dob\r\nJoe; 32; 31.10.1980\r\n");
        }
    }
}