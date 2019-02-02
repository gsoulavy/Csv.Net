namespace GSoulavy.Csv.Tests.Models {
    using System;

    [CsvFile(HasHeaders = true, Separator = ';')]
    public class FileWithHeaderDateAndSemi
    {
        [CsvColumn("name")]
        public string Name { get; set; }

        [CsvColumn("age")]
        public int Age { get; set; }

        [CsvIgnore]
        public string Title { get; set; }

        [CsvColumn("dob", typeof(CustomDateTimeConversion))]
        public DateTime DateOfBirth { get; set; }
    }
}