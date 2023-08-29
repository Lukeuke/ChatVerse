namespace File.Domain.Helpers;

public class ExtensionRecognition<TEnum>
{
    public string? Recognise(string fileName)
    {
        var split = fileName.Split(".");

        var extension = split[^1];
        
        try
        {
            var result = Enum.Parse(typeof(TEnum), Capitalize(extension));
            return result.ToString();
        }
        catch
        {
            return null;
        }
    }

    private static string Capitalize(string value)
    {
        var c = (char)(value[0] - 32);

        var trimStart = value.TrimStart(value[0]);

        return $"{c}{trimStart}";
    }
}