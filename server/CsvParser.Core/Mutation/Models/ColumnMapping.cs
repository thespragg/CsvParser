// ReSharper disable MemberCanBePrivate.Global
namespace CsvParser.Core.Mutation.Models;

public class ColumnMapping
{
    public string SourceColumn { get; }
    public string TargetColumn { get; }
    public bool Required { get; }
    private readonly Dictionary<string, string?> _lastValues;
    private bool Manual { get; }
    private string Template { get; }

    public ColumnMapping(
        string sourceColumn,
        string targetColumn,
        bool required = false,
        bool manual = false,
        string template = "${value}")
    {
        SourceColumn = sourceColumn;
        TargetColumn = targetColumn;
        Required = required;
        Manual = manual;
        Template = template;
        _lastValues = new Dictionary<string, string?>();
    }

    public string? GetValue(IReadOnlyDictionary<string, string?> row)
    {
        var success = row.TryGetValue(SourceColumn, out var colVal);
        if (!success) throw new Exception("Failed to map row.");

        var originalColumn = colVal;
        if (!Required) return StringFactory.Format(colVal, Template!);
        if (!string.IsNullOrEmpty(colVal) && !Manual) return StringFactory.Format(colVal, Template!);

        Console.WriteLine("A value must be provided for this column.");
        Console.WriteLine(
            $"SourceColumn: {SourceColumn}, DestinationColumn: {TargetColumn}");
        Console.WriteLine($"Row Data: {string.Join(",", row.Values)}");

        var hasExisting = colVal is not null && _lastValues.ContainsKey(colVal);
        if (hasExisting)
            Console.WriteLine($"Press enter to use the previous answer: {_lastValues[colVal!]}");

        do
        {
            colVal = Console.ReadLine()?.Trim();
        } while (!(hasExisting && string.IsNullOrEmpty(colVal)) && string.IsNullOrEmpty(colVal));

        if (!_lastValues.ContainsKey(originalColumn!) && !string.IsNullOrEmpty(originalColumn))
            _lastValues.Add(originalColumn, colVal);
        if (!string.IsNullOrEmpty(originalColumn) && !string.IsNullOrEmpty(colVal))
            _lastValues[originalColumn] = colVal;
        if (string.IsNullOrEmpty(colVal) && !string.IsNullOrEmpty(originalColumn) &&
            _lastValues.TryGetValue(originalColumn, out var value)) colVal = value;

        return colVal;
    }
}