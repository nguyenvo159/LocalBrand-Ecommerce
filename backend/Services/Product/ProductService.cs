﻿using System.Linq;
using AutoMapper;
using backend.Dto.Common;
using backend.Dto.Product;
using backend.Dto.Size;
using backend.Dtos.Product;
using backend.Entity;
using backend.Repositories;
using backend.Text.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace backend.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IRepository<Product> _rpProduct;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<ProductImage> _productImageRepository;
    private readonly IRepository<Size> _sizeRepository;
    private readonly IRepository<ProductInventory> _productInventoryRepository;
    private readonly IRepository<OrderItem> _rpOrderItem;
    private readonly ICloudService _cloudinaryService;

    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository,
        IRepository<Category> categoryRepository,
        IRepository<ProductImage> productImageRepository,
        IRepository<Size> sizeRepository1,
        IRepository<ProductInventory> productInventoryRepository,
        IRepository<OrderItem> rpOrderItem,
        ICloudService cloudinaryService,
        IRepository<Product> rpProduct,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _productImageRepository = productImageRepository;
        _sizeRepository = sizeRepository1;
        _productInventoryRepository = productInventoryRepository;
        _rpOrderItem = rpOrderItem;
        _cloudinaryService = cloudinaryService;
        _rpProduct = rpProduct;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        if (products.Count == 0 || products == null)
        {
            throw new Exception("Not found any product");
        }
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto> GetById(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new ApplicationException("Product not found");
        }
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<List<ProductDto>> GetByCategory(string categoryName)
    {
        var category = _categoryRepository.FindAsync(x => x.Name == categoryName).Result;
        if (category == null)
        {
            throw new ApplicationException("Category is not valid");
        }
        var products = await _productRepository.GetByCategoryAsync(category.Id);
        if (products == null)
        {
            throw new ApplicationException("Product not found");
        }

        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<List<string?>> GetImageByProductId(Guid productId)
    {
        var imgUrls = await _productImageRepository.FindAllAsync(i => i.ProductId == productId);
        if (imgUrls == null || imgUrls.Count == 0)
        {
            throw new ApplicationException("Image not found");
        }
        return imgUrls.Select(i => i.ImageUrl).Where(url => url != null).ToList();
    }

    public async Task<ProductDto> Create(ProductCreateDto productDto)
    {
        var existsProduct = await _productRepository.GetByNameAsync(productDto.Name);
        if (existsProduct != null)
        {
            throw new ApplicationException("Product is exist.");
        }
        var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId);
        if (category == null)
        {
            throw new ApplicationException("Category is not valid");
        }
        var product = _mapper.Map<Product>(productDto);

        var result = await _productRepository.CreateAsync(product);
        if (result == null)
        {
            throw new ApplicationException("Product not created");
        }

        //Add img
        if (productDto.ImageUrl != null && productDto.ImageUrl.Any())
        {
            var productImages = productDto.ImageUrl.Select(url => new ProductImage
            {
                ProductId = product.Id,
                ImageUrl = url.Trim()
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
                var size = await _sizeRepository.FindAsync(s => s.Name == sizeDto.Name);

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

    public async Task<ProductDto> Update(ProductUpdateDto productDto)
    {
        var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId);
        if (category == null)
        {
            throw new ApplicationException("Category is not valid");
        }
        var existsProduct = await _productRepository.GetByIdAsync(productDto.Id);
        if (existsProduct == null)
        {
            throw new ApplicationException("Product not found");
        }

        var product = _mapper.Map<Product>(productDto);
        var updatedProduct = await _productRepository.UpdateAsync(product);

        // Update product details
        if (productDto.Sizes != null && productDto.Sizes.Any() && updatedProduct != null)
        {
            foreach (var sizeDto in productDto.Sizes)
            {
                var size = await _sizeRepository.FindAsync(s => s.Name == sizeDto.Name);
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
                    else
                    {
                        existingInventory.Inventory = sizeDto.Inventory;
                        await _productInventoryRepository.UpdateAsync(existingInventory);
                    }
                }
                else
                {
                    throw new ApplicationException("Size not found");
                }
            }
        }

        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task<bool> Delete(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new ApplicationException("Product not found");
        }
        foreach (var productImage in product.ProductImages)
        {
            if (productImage.ImageUrl != null)
                await _cloudinaryService.DeleteImageAsync(productImage.ImageUrl);

        }
        return await _productRepository.DeleteAsync(id);
    }

    //Import
    public async Task<(int successCount, List<string> successNames, List<string> failedNames)> Import(IFormFile file)
    {
        var successCount = 0;
        var failedNames = new List<string>();
        var successNames = new List<string>();

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var name = worksheet.Cells[row, 1].Text;
                    var description = worksheet.Cells[row, 2].Text;
                    var price = decimal.TryParse(worksheet.Cells[row, 3].Text.Trim(), out decimal priceValue) ? priceValue : 0;

                    //Category
                    var categoryName = worksheet.Cells[row, 4].Text.Trim().ToLower();
                    var categoryId = await _categoryRepository.FindAsync(c => c.Name == categoryName).ContinueWith(t => t.Result?.Id) ?? Guid.Empty;
                    var imageUrl = worksheet.Cells[row, 5].Text.Trim().Split(',').ToList();
                    var sizes = new List<SizeDto>();

                    for (int col = 6; col <= 12; col++)
                    {
                        var sizeName = worksheet.Cells[1, col].Text.ToLower();
                        var inventory = int.TryParse(worksheet.Cells[row, col].Text.Trim(), out int inventoryValue) ? inventoryValue : -1;

                        if (inventory >= 0)
                        {
                            sizes.Add(new SizeDto { Name = sizeName, Inventory = inventory });
                        }
                    }

                    var productDto = new ProductCreateDto
                    {
                        Name = name,
                        Description = description,
                        Price = price,
                        CategoryId = categoryId,
                        ImageUrl = imageUrl,
                        Sizes = sizes
                    };

                    try
                    {
                        var temp = await Create(productDto);
                        if (temp != null)
                        {
                            successCount++;
                            successNames.Add(name);
                        }
                        else
                            failedNames.Add(name);
                    }
                    catch (Exception)
                    {
                        failedNames.Add(productDto.Name);
                    }
                }
            }
        }

        return (successCount, successNames, failedNames);
    }

    public async Task<List<ProductDto>> Search(string keyword)
    {
        var products = await _productRepository.SearchAsync(keyword);
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<List<ProductDto>> GetProductByRate()
    {
        var products = await _productRepository.GetByRate();
        if (products == null)
        {
            throw new ApplicationException("Product not found");
        }
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<List<ProductDto>> GetProductBestSell()
    {
        var products = await _productRepository.GetBestSell();
        if (products == null)
        {
            throw new ApplicationException("Product not found");
        }
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<PageResult<ProductDto>> GetPagingProduct(ProductPagingDto request)
    {
        var query = _productRepository.AsQueryable();
        if (request.Special.HasValue)
        {
            query = query.Where(p => p.Percentage > 0);
        }
        if (!string.IsNullOrEmpty(request.CategoryName))
        {
            query = query.Where(p => p.Category != null && p.Category.Name == request.CategoryName);
        }
        if (!string.IsNullOrEmpty(request.Search))
        {
            var searchTerm = request.Search.ToLower(); // Chuyển đổi từ tìm kiếm sang chữ thường
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm)
                                       || (p.Category != null && p.Category.Name.ToLower().Contains(searchTerm))
                                       || p.Description.ToLower().Contains(searchTerm));
        }
        var totalRecords = await query.CountAsync();
        query = query.OrderByDescending(p => p.CreatedAt);

        if (request.SortBy.HasValue)
        {
            if (request.SortBy.Value == Enums.SortBy.Newest)
            {
                query = query.OrderByDescending(p => p.UpdatedAt);
            }
            else if (request.SortBy.Value == Enums.SortBy.Popular)
            {
                query = query.OrderByDescending(p => p.OrderItems.Count(b => b.Order != null ? b.Order.Status == Enums.OrderStatus.Done : false))
                .ThenByDescending(p => p.UpdatedAt);
            }
            else if (request.SortBy.Value == Enums.SortBy.Related)
            {
                query = query.OrderBy(p => p.Name); // need action
            }
        }

        if (request.OrderByPrice.HasValue)
        {
            if (request.OrderByPrice.Value == Enums.OrderByPrice.Asc)
            {
                query = query.OrderBy(p => p.Price).ThenByDescending(p => p.UpdatedAt);
            }
            else if (request.OrderByPrice.Value == Enums.OrderByPrice.Desc)
            {
                query = query.OrderByDescending(p => p.Price).ThenByDescending(p => p.UpdatedAt);
            }
            else if (request.OrderByPrice.Value == Enums.OrderByPrice.InUp)
            {
                query = query.OrderBy(p => p.Sizes.Sum(s => s.Inventory)).ThenByDescending(p => p.UpdatedAt);
            }
            else if (request.OrderByPrice.Value == Enums.OrderByPrice.InDown)
            {
                query = query.OrderByDescending(p => p.Sizes.Sum(s => s.Inventory)).ThenByDescending(p => p.UpdatedAt);
            }
        }

        if (!request.PageSize.HasValue || !request.PageNumber.HasValue)
        {
            var result = await query.ToListAsync();
            return new PageResult<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(result),
                TotalRecords = totalRecords
            };
        }
        var products = await query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                                  .Take(request.PageSize.Value)
                                  .ToListAsync();
        return new PageResult<ProductDto>
        {
            Items = _mapper.Map<List<ProductDto>>(products),
            PageNumber = request.PageNumber.Value,
            PageSize = request.PageSize.Value,
            TotalRecords = totalRecords
        };
    }

    public Task<ProductAnalytics> ProductAnalytics()
    {
        var products = _productRepository.AsQueryable();
        var totalProducts = products.Count();
        var now = DateTime.UtcNow;
        var items = _rpOrderItem.AsQueryable()
        .Where(x => x.Order.Status != Enums.OrderStatus.Done && x.Product != null);
        var totalProductsSold = items.Where(x => x.Order.CreatedAt.Date == now.Date).Sum(x => x.Quantity);
        var totalProductSoldForWeek = items.Where(x => x.Order.CreatedAt.Date >= now.Date.AddDays(-7)).Sum(x => x.Quantity);
        var totalProductSoldForMonth = items.Where(x => x.Order.CreatedAt.Date >= now.Date.AddMonths(-1)).Sum(x => x.Quantity);
        var result = new ProductAnalytics
        {
            TotalProduct = totalProducts,
            TotalProductSold = totalProductsSold,
            TotalProductSoldForWeek = totalProductSoldForWeek,
            TotalProductSoldForMonth = totalProductSoldForMonth
        };
        return Task.FromResult(result);
    }

    public async Task UpdateSpecial(ProductSpecialUpdate request)
    {
        foreach (var productId in request.ProductIds)
        {
            var product = await _rpProduct.AsQueryable().FirstOrDefaultAsync(x => x.Id == productId);
            if (product != null)
            {
                product.Percentage = request.Percentage;
                await _rpProduct.UpdateAsync(product);
            }
        }
    }
}


