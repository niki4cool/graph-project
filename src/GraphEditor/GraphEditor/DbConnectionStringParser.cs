using System.Text.RegularExpressions;

namespace GraphEditor;

public static class DbConnectionStringConverter
{
    private static readonly Regex urlConnectionStringRegex =
        new Regex(@"\/\/(?<Username>\w+):(?<Password>\w+)@(?<Host>.+):(?<Port>\d+)\/(?<Database>\w+)",
            RegexOptions.Singleline);

    public static string FromUrlToKeyValue(string urlConnectionString)
    {
        var match = urlConnectionStringRegex.Match(urlConnectionString);
        return
            $"Host={match.Groups["Host"]};" +
            $"Port={match.Groups["Port"]};" +
            $"Database={match.Groups["Database"]};" +
            $"Username={match.Groups["Username"]};" +
            $"Password={match.Groups["Password"]}";
    }
}