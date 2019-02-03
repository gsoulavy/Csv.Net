# GSoulavy.Csv.Net

[![NuGet version (GSoulavy.Csv.Net)](https://img.shields.io/nuget/v/GSoulavy.Csv.Net.svg?style=flat-square)](https://www.nuget.org/packages/GSoulavy.Csv.Net/) [![Build status](https://gsoulavy.visualstudio.com/CI/_apis/build/status/Master)](https://gsoulavy.visualstudio.com/CI/_build/latest?definitionId=-1)

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

Mapping (v1.1):
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

        [CsvColumn("dob", Format = "yyyy-M-d")]
        public DateTime DateOfBirth { get; set; }
    }
```
Mapping (v2.0)

You need to implement the ICustomConversion interface in order to achieve custom conversion and pass as a type into your CsvColumnAttribute

The `ICustomConversion` inteface:
```cs
    public interface ICustomConversion
    {
        object Parse(string value);

        string Compose(object value);
    }
```
The `ICustomConversion` inteface:
An example implementaion for example by you as `CustomDateTimeConversion`:
```cs
    public class CustomDateTimeConversion : ICustomConversion
    {
        private const string Format = "d.M.yyyy";

        public object Parse(string value)
        {
            return DateTime.TryParseExact(value, Format, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsedDate) ? parsedDate : DateTime.MinValue;
        }

        public string Compose(object value)
        {
            return ((DateTime) value).ToString(Format);
        }
    }
```
Your model declaration:
```cs
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
```
Desiralizing in action:
```cs
var list = CsvConvert.Deserialize<Person>(text).ToList();
```
Serializing in action:
```cs
var csv = CsvConvert.Serialize(list);
```
