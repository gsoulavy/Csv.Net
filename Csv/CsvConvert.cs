namespace GSoulavy.Csv
{
    using System.Collections.Generic;
    using System.IO;

    public static class CsvConvert
    {
        public static IEnumerable<T> Deserialize<T>(string csv) where T : class
        {
            return Serializer<T>
                .Create().Deserialize(csv);
        }

        public static IEnumerable<T> Deserialize<T>(Stream csvStream) where T : class
        {
            return Serializer<T>
                .Create().Deserialize(csvStream);
        }

        public static string Serialize<T>(IEnumerable<T> list) where T : class
        {
            return Serializer<T>
                .Create().Serialize(list);
        }
    }
}