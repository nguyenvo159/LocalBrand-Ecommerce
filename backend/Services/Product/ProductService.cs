using AutoMapper;
using backend.Dto.Product;
using backend.Entity;
using backend.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<ProductImage> _productImageRepository;
    private readonly ISizeRepository _sizeRepository;
    private readonly IRepository<ProductInventory> _productInventoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository,
        IRepository<Category> categoryRepository,
        IRepository<ProductImage> productImageRepository,
        ISizeRepository sizeRepository,
        IRepository<ProductInventory> productInventoryRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _productImageRepository = productImageRepository;
        _sizeRepository = sizeRepository;
        _productInventoryRepository = productInventoryRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto> GetById(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return null;
        return _mapper.Map<ProductDto>(product);
    }
    public async Task<ProductDto> Create(ProductCreateDto productDto)
    {

        var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId);
        if (category == null)
        {
            throw new ApplicationException("Category not found");
        }
        var product = _mapper.Map<Product>(productDto);

        var result = await _productRepository.CreateAsync(product);

        //Add img
        if (productDto.ImageUrl != null && productDto.ImageUrl.Any())
        {
            var productImages = productDto.ImageUrl.Select(url => new ProductImage
            {
                ProductId = product.Id,
                ImageUrl = url
            }).ToList();

            foreach (var image in productImages)
            {
                await _productImageRepository.AddAsync(image);
            }
        }

        // Add Inventory
        if (productDto.Sizes != null && productDto.Sizes.Any())
        {
            var productInventoryList = new List<ProductInventory>();

            foreach (var sizeDto in productDto.Sizes)
            {
                // Find size by Name in the repository
                var size = await _sizeRepository.GetByNameAsync(sizeDto.Name);

                if (size != null)
                {
                    var productInventory = new ProductInventory
                    {
                        ProductId = product.Id,
                        SizeId = size.Id,
                        Inventory = sizeDto.Inventory
                    };

                    productInventoryList.Add(productInventory);
                }
            }

            foreach (var inventory in productInventoryList)
            {
                await _productInventoryRepository.AddAsync(inventory);
            }
        }
        return _mapper.Map<ProductDto>(result);

    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductDto> Update(ProductCreateDto productDto)
    {
        throw new NotImplementedException();
    }
}
