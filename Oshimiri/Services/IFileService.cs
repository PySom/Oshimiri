namespace Oshimiri.Services;

public interface IFileService
{
    void RemoveImageAsync(string? imageUrl);
    Task TryUploadFileAsync(IFormFile file, out string? fileName);
}