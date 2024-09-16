
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


    public async Task<string> UploadImageAsync(IFormFile file)
    {

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, file.OpenReadStream())
        };

        var url = await _cloudinary.UploadAsync(uploadParams);


        return url.SecureUrl.ToString();
    }


    public async Task DeleteImageAsync(string url)
    {
        var uri = new Uri(url);
        var segments = uri.Segments;
        var publicIdWithExtension = segments.Last();
        var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);

        var deletionParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deletionParams);
    }


}
