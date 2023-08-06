// ReSharper disable MemberCanBePrivate.Global
namespace CsvParser.Core.Mutation.Models;

public class ValueMapping
{
    public string[] InputColumns { get; }
    public string OutputColumn { get; }
    public string SearchValue { get; }
    public string? Replacement { get; }
    public bool CaseSensitive { get; }
    public bool Contains { get; }
    public string Template { get; }
    public string? ReplacementCol { get; }
    public ValueMapping(
        string[] inputColumns,
        string searchValue,
        string? replacement,
        string? replacementCol,
        string outputColumn,
        bool caseSensitive,
        bool contains,
        string template = "@${value}"
    )
    {
        SearchValue = searchValue;
        Replacement = replacement;
        OutputColumn = outputColumn;
        InputColumns = inputColumns;
        CaseSensitive = caseSensitive;
        Contains = contains;
        ReplacementCol = replacementCol;
        Template = template;
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

            if (Contains ? !doesContain : !isEqual) continue;
            
            if (Replacement is not null) return Replacement;
            if (ReplacementCol is null) throw new Exception("A replacement value or column must be specified.");
            var success = row.TryGetValue(ReplacementCol ?? "", out var val);
            if (!success) throw new Exception("Failed to find replacement column");
            return StringFactory.Format(val, Template);
        }

        return null;
    }
}