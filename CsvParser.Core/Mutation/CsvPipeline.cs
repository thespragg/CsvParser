using System.Text.Json;
using System.Text.Json.Serialization;
using CsvParser.Core.Mutation.Models;

namespace CsvParser.Core.Mutation;

public class CsvPipeline
{
    private readonly List<string> _newColumnNames;
    private readonly List<ColumnMapping> _columnMappings;
    private readonly List<ValueMapping> _valueMappings;
    
    [JsonPropertyName("ColumnMappings")] public IEnumerable<ColumnMapping> ColumnMappings => _columnMappings;
    [JsonPropertyName("ValueMappings")] public IEnumerable<ValueMapping> ValueMappings => _valueMappings;

    private CsvPipeline(List<string> newColumnNames)
    {
        _newColumnNames = newColumnNames;
        _columnMappings = new List<ColumnMapping>();
        _valueMappings = new List<ValueMapping>();
    }

    public static CsvPipeline FromColumns(List<string> newColumnNames)
        => new(newColumnNames);

    public CsvPipeline MapColumn(
        string sourceColumn,
        string targetColumn,
        bool required = false,
        bool manual = false
    )
    {
        if (_columnMappings.Any(x => x.TargetColumn == targetColumn))
        {
            throw new ArgumentException($"Target column '{targetColumn}' already exists.");
        }

        _columnMappings.Add(new ColumnMapping(sourceColumn, targetColumn, required, manual));
        return this;
    }

    public CsvPipeline MapValue(
        string[] inputColumns,
        string outputColumn,
        string searchValue,
        string replacementValue,
        bool caseSensitive = true,
        bool contains = false
    )
    {
        var mapping = new ValueMapping(inputColumns, searchValue, replacementValue, outputColumn, caseSensitive, contains);

        if (mapping.InputColumns.Length == 0 || string.IsNullOrEmpty(mapping.Replacement) ||
            string.IsNullOrEmpty(mapping.OutputColumn))
        {
            throw new ArgumentException("Replacement, Input Columns and Output Column are all required.");
        }

        _valueMappings.Add(mapping);
        return this;
    }

    public CsvDataWrapper Run(CsvDataWrapper sourceData)
    {
        var newData = new List<Dictionary<string, string?>>();

        foreach (var row in sourceData)
        {
            var newRow = new Dictionary<string, string?>();
            foreach (var mapping in _columnMappings)
            {
                newRow[mapping.TargetColumn] = mapping.GetValue(row);
            }

            foreach (var mapping in _valueMappings)
            {
                var val = mapping.GetValue(row);
                if (val == null) continue;
                newRow[mapping.OutputColumn] = val;
            }

            newData.Add(newRow);
        }

        return new CsvDataWrapper(newData, new List<string>(_newColumnNames));
    }

    public void Save(string path)
    {
        File.WriteAllText(path, JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }

    public static CsvPipeline Load(string path)
    {
        var input = File.ReadAllText(path);
        return JsonSerializer.Deserialize<CsvPipeline>(input)!;
    }
}