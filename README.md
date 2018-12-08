# GSoulavy.Csv.Net

[![NuGet version (Newtonsoft.Json)](https://img.shields.io/nuget/v/GSoulavy.Csv.Net.svg?style=flat-square)](https://www.nuget.org/packages/GSoulavy.Csv.Net/)

Simple csv .Net Standard 2.0 parser library

## Usage

### Attributes

#### ClassAttribute:
`CsvFileAttribute` fileAttribute has three parameters:
 - HasHeaders (`bool`): declares if the input file has header or not,
 - Separator (`char`): define the column separator,
 - StringQuotes (`char`): declares the quotation -> default is `'\0'` (it will trim the given value when it's declared e.g `"` or `'`)

#### PropertyAttribute
 `CsvColumnAttribute` has two properties:
- Name (`string`): mapping name of the csv header,
- Position (`int`): the position of the column

Source file:
```
name, age
Gabs, 35
Joe, 40
```

Mapping:
 ```cs
    [CsvFile(HasHeaders = true, Separator = ',')]
    public class Person
    {
        [CsvColumn("name")]
        public string Name { get; set; }

        [CsvColumn("age")]
        public int Age { get; set; }

        [CsvIgnore]
        public string Title { get; set; }
    }
```
Desiralizing in action:
```cs
var list = CsvConvert.Deserialize<Person>(text).ToList();
```
Serializing in action:
```cs
var csv = CsvConvert.Serialize(list);
```