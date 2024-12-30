using System.Globalization;
using CsvHelper;

namespace Backend.Services;

public class FileService
{
    public async Task<int> ProcessCsvFile(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<dynamic>().ToList();
        // TODO: Process records
        await Task.CompletedTask;

        return records.Count;
    }
}