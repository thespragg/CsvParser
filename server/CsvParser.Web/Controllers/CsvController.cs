using System.Text.Json;
using CsvParser.Core;
using CsvParser.Core.Mutation;
using CsvParser.Core.Mutation.Models;
using CsvParser.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CsvParser.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvController : ControllerBase
{
    [HttpPost]
    public IActionResult Map(MapDto mapSettings)
    {
        var pipeline = CsvPipeline.FromSettings(mapSettings.Settings);
        var data = CsvDataWrapper.FromFile(mapSettings.FilePath);
        var res = pipeline.Run(data);

        var (path, filename, filetype) = GetFileDetails(mapSettings.OutputPath);
        res.SaveToFile(path, filename ?? $"MappedCsv{DateTime.Now:dd/MM/yyyy}", filetype ?? ".csv");
        return Ok(mapSettings.OutputPath);
    }

    private static (string, string?, string?) GetFileDetails(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (string.IsNullOrEmpty(directory)) throw new Exception("Failed to parse output directory");

        var fileName = Path.GetFileName(filePath);
        var fileType = Path.GetExtension(filePath);

        return (directory, string.IsNullOrEmpty(fileName) ? null : fileName,
            string.IsNullOrEmpty(fileType) ? null : fileType);
    }
}