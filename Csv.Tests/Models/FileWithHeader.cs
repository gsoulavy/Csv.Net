namespace GSoulavy.Csv.Tests.Models
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

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

    [CsvFile(HasHeaders = true, Separator = ',')]
    public class FileWithHeaderAndDate
    {
        [CsvColumn("name")]
        public string Name { get; set; }

        [CsvColumn("age")]
        public int Age { get; set; }

        [CsvIgnore]
        public string Title { get; set; }

        [CsvColumn("dob", Format = "yyyy-M-d")]
        public DateTime DateOfBirth { get; set; }
    }

}