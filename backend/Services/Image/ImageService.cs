using System;
using System.Diagnostics;
using System.Text.Json;
using AutoMapper;
using backend.Dto.Product;
using backend.Entity;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;
using OpenCvSharp;
using Pgvector;
using Pgvector.EntityFrameworkCore;


namespace backend.Services;

public class ImageService : IImageService
{
    private readonly IRepository<ProductImage> _productImageRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICloudService _cloudService;
    private readonly IMapper _mapper;
    public ImageService(IRepository<ProductImage> productImageRepository,
        IProductRepository productRepository,
        ICloudService cloudService,
        IMapper mapper)
    {
        _productImageRepository = productImageRepository;
        _productRepository = productRepository;
        _cloudService = cloudService;
        _mapper = mapper;
    }

    public async Task UploadImage(List<IFormFile> files, Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            throw new ApplicationException("Product not found");
        }
        foreach (var file in files)
        {
            var url = await _cloudService.UploadImageAsync(file);

            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            Vector? vectorStr = await GetImageVectorAsync(filePath);

            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = url,
                ImageVector = vectorStr
            };
            await _productImageRepository.AddAsync(productImage);
            File.Delete(filePath);

        }
    }

    public async Task<Vector> GetImageVectorAsync(string imagePath)
    {
        var pythonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "PyScript", "ConvertImage.py");
        var psi = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = $"\"{pythonFilePath}\" \"{imagePath}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        var output = await process.StandardOutput.ReadToEndAsync();

        process.WaitForExit();

        var vectorArray = JsonSerializer.Deserialize<float[]>(output);
        if (vectorArray == null)
        {
            return null;
        }

        var vectorPg = new Vector(vectorArray);
        return vectorPg;
    }


    public async Task<List<ProductDto>> SearchImageAsync(IFormFile file)
    {
        var filePath = Path.GetTempFileName();
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        Vector? vectorStr = await GetImageVectorAsync(filePath);
        var items = await _productImageRepository.AsQueryable()
            .Include(x => x.Product)
            .OrderBy(x => x.ImageVector!.L2Distance(vectorStr))
            .Take(1) // Lấy 1 sản phẩm gần nhất
            .ToListAsync();
        return _mapper.Map<List<ProductDto>>(items);
    }


}
