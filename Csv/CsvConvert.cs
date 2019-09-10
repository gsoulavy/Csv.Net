namespace GSoulavy.Csv
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public static class CsvConvert
    {
        public static IEnumerable<T> Deserialize<T>(string csv) where T : class
        {
            return Serializer<T>
                .Create().Deserialize(csv);
        }

        public static IEnumerable<T> Deserialize<T>(Stream csvStream, Encoding encoding = null) where T : class
        {
            return Serializer<T>
                .Create().Deserialize(csvStream, encoding);
        }

        public static string Serialize<T>(IEnumerable<T> list) where T : class
        {
            return Serializer<T>
                .Create().Serialize(list);
        }
    }
}