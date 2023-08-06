using System.Text.RegularExpressions;

namespace CsvParser.Core.Mutation;
public static partial class StringFactory
{
    public static string Format(string? value, string template)
    {
        var regex = FindValues();
        var matches = regex.Matches(template);

        foreach (Match match in matches)
        {
            var modifiers = match.Groups[1].Value.ToLower().Split(':');
            var mutated = modifiers.Aggregate(value, (current, modifier) =>
                modifier switch
                {
                    not null when modifier.Equals("upper", StringComparison.InvariantCultureIgnoreCase) => current?.ToUpper() ?? "",
                    not null when modifier.Equals("lower", StringComparison.InvariantCultureIgnoreCase) => current?.ToLower() ?? "",
                    not null when modifier.Equals("first", StringComparison.InvariantCultureIgnoreCase) 
                        => current?.FirstOrDefault() is { } c ? c == default ? "" : c.ToString() : "",
                    not null when modifier.Contains("replace", StringComparison.InvariantCultureIgnoreCase) => PerformReplacement(current, modifier),
                    _ => current
                }
            );

            template = template.Replace(match.Value, mutated);
        }

        return template;
    }

    private static string? PerformReplacement(string? value, string replaceString)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(replaceString))
            return value;

        var match = FindReplacements().Match(replaceString);
        if (!match.Success || match.Groups.Count < 3)
            return value;

        var oldString = match.Groups[1].Value;
        var newString = match.Groups[2].Value;

        return value.Replace(oldString, newString);
    }

    [GeneratedRegex(@"\$\{[vV]alue(?::(.*?))?\}")]
    private static partial Regex FindValues();
    [GeneratedRegex(@"replace\[(.*?)\]\[(.*?)\]")]
    private static partial Regex FindReplacements();
}