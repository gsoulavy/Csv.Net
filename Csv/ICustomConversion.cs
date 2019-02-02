namespace GSoulavy.Csv
{
    public interface ICustomConversion
    {
        object Parse(string value);

        string Compose(object value);
    }
}