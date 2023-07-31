namespace CsvParser.Core.Extensions;

public static class CsvDataWrapperExtensions
{
    public static void AppendFile(this CsvDataWrapper csvData, string filePath)
    {
        var newData = new List<Dictionary<string, string?>>();

        using (var reader = new StreamReader(filePath))
        {
            var headers = reader.ReadLine()?.Split(',');

            if (headers == null)
            {
                throw new Exception("CSV file is empty or invalid.");
            }

            var columnNames = headers.ToList();

            if (!columnNames.SequenceEqual(csvData.GetColumnNames()))
            {
                Console.WriteLine("Column headers in the file do not match the existing data.");
                Console.WriteLine("Edit your csv to match the headings.");
                return;
            }

            var newColumnNames = csvData.GetColumnNames().ToList();

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
                    rowDict[newColumnNames[i]] = values[i];
                }

                newData.Add(rowDict);
            }
        }

        csvData.Append(newData);
    }
}