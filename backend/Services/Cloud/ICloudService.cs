namespace backend.Services;

public interface ICloudService
{
    Task<string> UploadImageAsync(IFormFile file);
}
