namespace CsvParser.Core.Utils;

public class CsvPrinter
{
    private readonly List<Dictionary<string, string?>> _data;
    private readonly List<string> _columnNames;

    public CsvPrinter(List<Dictionary<string, string?>> data, List<string> columnNames)
    {
        _data = data;
        _columnNames = columnNames;
    }

    public void PrintLines(int start, int rows)
    {
        Console.WriteLine("Column Names:");
        foreach (var columnName in _columnNames)
        {
            Console.Write(columnName.PadRight(GetColumnWidth(columnName)) + "\t");
        }
        Console.WriteLine();

        for (var i = start; i < start + rows; i++)
        {
            var row = _data[i - 1];
            foreach (var columnName in _columnNames)
            {
                var value = row.TryGetValue(columnName, out var val) ? val : string.Empty;
                Console.Write(value?.PadRight(GetColumnWidth(columnName)) + "\t");
            }
            Console.WriteLine();
        }
    }

    private int GetColumnWidth(string columnName)
    {
        var max = columnName.Length;
        foreach (var row in _data)
        {
            if (row.TryGetValue(columnName, out var value) && value?.Length > max)
            {
                max = value.Length;
            }
        }
        return max;
    }
}
