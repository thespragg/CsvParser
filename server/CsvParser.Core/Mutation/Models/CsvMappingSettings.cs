using System.Text.Json;

namespace CsvParser.Core.Mutation.Models;

public class CsvMappingSettings
{
    public List<string> NewColumnNames { get; init; } = new();
    public List<ColumnMapping> ColumnMappings { get; } = new();
    public List<ValueMapping> ValueMappings { get; } = new();
}