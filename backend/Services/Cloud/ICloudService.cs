namespace backend.Services;

public interface ICloudService
{
    Task<string> UploadImageAsync(IFormFile file);
    Task DeleteImageAsync(string url);
}
