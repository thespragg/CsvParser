using System.Collections;
using CsvParser.Core.Utils;

namespace CsvParser.Core;

public class CsvDataWrapper : IEnumerable<Dictionary<string, string?>>
{
    private readonly List<Dictionary<string, string?>> _data;
    private readonly List<string> _columnNames;

    public CsvDataWrapper(List<Dictionary<string, string?>> data, List<string> columnNames)
    {
        _data = data;
        _columnNames = columnNames;
    }

    public static CsvDataWrapper FromFile(string filePath)
    {
        var (data, columnNames) = CsvParser.ParseFile(filePath);
        return new CsvDataWrapper(data, columnNames);
    }

    public void PrintLines(int start, int rows)
    {
        var printer = new CsvPrinter(_data, _columnNames);
        printer.PrintLines(start, rows);
    }

    public void Append(List<Dictionary<string, string?>> rows)
    {
        foreach (var row in rows)
        {
            _data.Add(row);
        }
    }

    public void SaveToFile(string path, string fileName, string fileType = "csv")
    {
        if (!fileType.StartsWith(".")) fileType = "." + fileType;

        var fullPath = Path.Combine(path, fileName + fileType);

        using var writer = new StreamWriter(fullPath);
        writer.WriteLine(string.Join(",", _columnNames));

        foreach (var row in _data)
        {
            var values = new List<string>();
            foreach (var columnName in _columnNames)
            {
                if (row.TryGetValue(columnName, out var value))
                {
                    values.Add(value ?? "");
                }
                else
                {
                    values.Add("");
                }
            }
            writer.WriteLine(string.Join(",", values));
        }
    }

    public IEnumerable<string> GetColumnNames()
        => _columnNames;

    public IEnumerator<Dictionary<string, string?>> GetEnumerator()
        => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}