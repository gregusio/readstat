using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[Consumes("multipart/form-data")]
public class FileController(FileService fileService) : ControllerBase
{
    private readonly FileService _fileService = fileService;

    [HttpPost("upload-csv")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { Message = "No file uploaded" });
        }

        if (!Path.GetExtension(file.FileName).ToLower().Equals(".csv"))
        {
            return BadRequest(new { Message = "Only CSV files are allowed" });
        }

        try
        {
            var result = await _fileService.ProcessCsvFile(file);
            return Ok(new { Message = "File uploaded successfully", RowsProcessed = result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Error processing file", Details = ex.Message });
        }
    }
}