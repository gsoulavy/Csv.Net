namespace GSoulavy.Csv.Tests.Models 
 {
    using System;

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