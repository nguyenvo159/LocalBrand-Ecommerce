using System;
using backend.Dto.Product;
using backend.Dtos;
using backend.Entity;

namespace backend.Services;

public interface IImageService
{
    Task UploadImage(List<IFormFile> files, Guid productId);
    Task UploadImageNoVector(List<IFormFile> files, Guid productId);
    Task UpdateImage(ImageRequest request);
    Task<List<ProductDto>> SearchImageAsync(IFormFile file);
}
