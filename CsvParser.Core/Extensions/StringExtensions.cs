using System.Globalization;

namespace CsvParser.Core.Extensions;

public static class StringExtensions
{
    private static readonly char[] Separator = { ' ', '-', '_' };

    public static string ToTitleCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var words = value.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        for (var i = 0; i < words.Length; i++)
        {
            var word = words[i];
            if (!char.IsLower(word[0])) continue;
            
            if (word.Contains('_'))
                words[i] = textInfo.ToTitleCase(word.Replace('_', ' '));
            else if (word.Contains('-'))
                words[i] = textInfo.ToTitleCase(word.Replace('-', ' '));
            else if (i == 0 && char.IsUpper(word[0]))
                words[i] = textInfo.ToTitleCase(word);
            else
                words[i] = textInfo.ToTitleCase(word);
        }

        var result = string.Join("", words);
        return result;
    }
}