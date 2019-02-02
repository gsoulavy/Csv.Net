namespace GSoulavy.Csv.Tests.Files
{
    public interface ICustomConverter
    {
        object Parse(string value);

        string Compose(object value);
    }
}