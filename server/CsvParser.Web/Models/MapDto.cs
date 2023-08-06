using CsvParser.Core.Mutation.Models;

namespace CsvParser.Web.Models;

public class MapDto
{
    public string FilePath { get; set; } = null!;
    public string OutputPath { get; set; } = null!;
    public CsvMappingSettings Settings { get; set; } = null!;
}