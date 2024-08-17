namespace backend.Services;

public interface ICloudService
{
    Task<List<string>> UploadImageAsync(List<IFormFile> files);
}
