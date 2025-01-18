namespace Backend.Interfaces;

public interface IFileService
{
    Task<int> ProcessFile(IFormFile file, int userId);
}