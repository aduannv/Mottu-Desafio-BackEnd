using Mottu.Domain.Interfaces;

namespace Mottu.Application.Services;

public class FileService : IFileService
{
    private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "CNHImages");

    public FileService()
    {
        if (!Directory.Exists(_imagePath))
        {
            Directory.CreateDirectory(_imagePath);
        }
    }

    public async Task SaveCnhImageAsync(string base64Image, string fileName)
    {
        byte[] imageBytes = Convert.FromBase64String(base64Image);
        string filePath = Path.Combine(_imagePath, $"{fileName}.jpg");

        if (File.Exists(filePath))
            File.Delete(filePath);

        await File.WriteAllBytesAsync(filePath, imageBytes);
    }

    public async Task<string> GetCnhImage(string fileName)
    {
        string filePath = Path.Combine(_imagePath, $"{fileName}.jpg");

        if (File.Exists(filePath))
        {
            byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
            return Convert.ToBase64String(imageBytes);
        }

        return string.Empty;
    }
}
