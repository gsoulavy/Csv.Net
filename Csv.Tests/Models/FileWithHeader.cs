﻿namespace GSoulavy.Csv.Tests.Models
{
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
}