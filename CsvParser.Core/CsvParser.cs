namespace CsvParser.Core;

public abstract class CsvParser
{
    public static (List<Dictionary<string, string?>> data, List<string> columnNames) ParseFile(string filePath)
    {
        var data = new List<Dictionary<string, string?>>();
        List<string> columnNames;

        using (var reader = new StreamReader(filePath))
        {
            var headers = reader.ReadLine()?.Split(',');

            if (headers == null)
            {
                throw new Exception("CSV file is empty or invalid.");
            }

            columnNames = headers.ToList();

            while (reader.ReadLine() is { } line)
            {
                var values = line.Split(',');
                if (values.Length != headers.Length)
                {
                    throw new Exception("CSV file has inconsistent data.");
                }

                var rowDict = new Dictionary<string, string?>();
                for (var i = 0; i < headers.Length; i++)
                {
                    rowDict[headers[i]] = values[i];
                }

                data.Add(rowDict);
            }
        }

        return (data, columnNames);
    }
}