namespace Mottu.Domain.Interfaces
{
    public interface IFileService
    {
        Task SaveCnhImageAsync(string base64Image, string fileName);
        Task<string> GetCnhImage(string fileName);
    }
}