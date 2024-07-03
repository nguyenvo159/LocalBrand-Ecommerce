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
    private readonly IRepository<Size> _sizeRepository1;
    private readonly IRepository<ProductInventory> _productInventoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository,
        IRepository<Category> categoryRepository,
        IRepository<ProductImage> productImageRepository,
        IRepository<Size> sizeRepository1,
        IRepository<ProductInventory> productInventoryRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _productImageRepository = productImageRepository;
        _sizeRepository1 = sizeRepository1;
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
                var size = await _sizeRepository1.FindAsync(s => s.Name == sizeDto.Name);

                if (size != null)
                {
                    var existingInventory = await _productInventoryRepository.FindAsync(pi =>
                        pi.ProductId == product.Id && pi.SizeId == size.Id);

                    if (existingInventory == null)
                    {
                        var productInventory = new ProductInventory
                        {
                            ProductId = product.Id,
                            SizeId = size.Id,
                            Inventory = sizeDto.Inventory
                        };
                        await _productInventoryRepository.AddAsync(productInventory);
                    }
                }
            }
        }
        return _mapper.Map<ProductDto>(result);

    }

    public async Task<bool> Delete(Guid id)
    {
        return await _productRepository.DeleteAsync(id);
    }

    public async Task<ProductDto> Update(ProductUpdateDto productDto)
    {
        var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId);
        if (category == null)
        {
            throw new ApplicationException("Category not found");
        }

        var product = _mapper.Map<Product>(productDto);

        // Update product details
        var updatedProduct = await _productRepository.UpdateAsync(product);

        if (productDto.ImageUrl != null && productDto.ImageUrl.Any())
        {
            var productImages = productDto.ImageUrl.Select(url => new ProductImage
            {
                ProductId = updatedProduct.Id,
                ImageUrl = url
            }).ToList();

            foreach (var image in productImages)
            {
                await _productImageRepository.AddAsync(image);
            }
        }

        if (productDto.Sizes != null && productDto.Sizes.Any())
        {
            foreach (var sizeDto in productDto.Sizes)
            {
                var size = await _sizeRepository1.FindAsync(s => s.Name == sizeDto.Name);
                if (size != null)
                {
                    var existingInventory = await _productInventoryRepository.FindAsync(pi =>
                        pi.ProductId == product.Id && pi.SizeId == size.Id);

                    if (existingInventory == null)
                    {
                        var productInventory = new ProductInventory
                        {
                            ProductId = updatedProduct.Id,
                            SizeId = size.Id,
                            Inventory = sizeDto.Inventory
                        };
                        await _productInventoryRepository.AddAsync(productInventory);
                    }
                }
            }
        }

        return _mapper.Map<ProductDto>(updatedProduct);
    }
}
