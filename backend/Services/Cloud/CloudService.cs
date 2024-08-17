
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace backend.Services;

public class CloudService : ICloudService
{
    private readonly Cloudinary _cloudinary;

    public CloudService(IConfiguration configuration)
    {
        var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]);

        _cloudinary = new Cloudinary(account);
    }

    public async Task<List<string>> UploadImageAsync(List<IFormFile> files)
    {
        var uploadResult = new List<string>();
        foreach (var file in files)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            var url = await _cloudinary.UploadAsync(uploadParams);
            uploadResult.Add(url.SecureUrl.ToString());
        }

        return uploadResult;
    }
}
