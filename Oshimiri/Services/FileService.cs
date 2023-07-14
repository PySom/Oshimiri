namespace Oshimiri.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment environment;
    private const string AssetFolder = "Assets";
    public FileService(IWebHostEnvironment environment)
    {
        this.environment = environment;
    }
    public Task TryUploadFileAsync(IFormFile file, out string? fileName)
    {
        fileName = default;
        if(file is null) return Task.CompletedTask;
        fileName = Path.Combine(AssetFolder, $"{Guid.NewGuid().ToString().Replace("-", "")}.{Path.GetExtension(file.FileName)}");
        string path = Path.Combine(environment.WebRootPath, fileName);
        using FileStream fileStream = File.OpenWrite(path);
        return file.CopyToAsync(fileStream);
    }

    public void RemoveImageAsync(string? imageUrl)
    {
        if(imageUrl != null)
        {
            string path = Path.Combine(environment.WebRootPath, imageUrl);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
