// ReSharper disable MemberCanBePrivate.Global
namespace CsvParser.Core.Mutation.Models;

public class ValueMapping
{
    public string[] InputColumns { get; }
    public string OutputColumn { get; }
    public string SearchValue { get; }
    public string Replacement { get; }
    public bool CaseSensitive { get; }
    public bool Contains { get; }

    public ValueMapping(
        string[] inputColumns,
        string searchValue,
        string replacement,
        string outputColumn,
        bool caseSensitive,
        bool contains
    )
    {
        SearchValue = searchValue;
        Replacement = replacement;
        OutputColumn = outputColumn;
        InputColumns = inputColumns;
        CaseSensitive = caseSensitive;
        Contains = contains;
    }

    public string? GetValue(IReadOnlyDictionary<string, string?> row)
    {
        var comparisonType = !CaseSensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        foreach (var col in InputColumns)
        {
            if (!row.TryGetValue(col, out var value))
                throw new Exception("Failed to get column value");
            if (value is null) continue;

            var doesContain = value.Contains(SearchValue, comparisonType);
            var isEqual = value.Equals(SearchValue, comparisonType);

            if (Contains ? doesContain : isEqual)
                return Replacement;
        }

        return null;
    }
}