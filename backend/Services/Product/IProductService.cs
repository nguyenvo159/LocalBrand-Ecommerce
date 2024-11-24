using backend.Dto.Common;
using backend.Dto.Product;
using backend.Dtos.Product;

namespace backend.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAll();
    Task<ProductDto> GetById(Guid id);
    Task<List<string?>> GetImageByProductId(Guid productId);
    Task<List<ProductDto>> GetByCategory(string categoryName);
    Task<List<ProductDto>> GetProductByRate();
    Task<List<ProductDto>> GetProductBestSell();
    Task<PageResult<ProductDto>> GetPagingProduct(ProductPagingDto productPagingDto);
    Task<ProductDto> Create(ProductCreateDto productDto);
    Task<ProductDto> Update(ProductUpdateDto productDto);
    Task<bool> Delete(Guid id);
    Task<List<ProductDto>> Search(string keyword);

    Task<(int successCount, List<string> successNames, List<string> failedNames)> Import(IFormFile file);

    Task<ProductAnalytics> ProductAnalytics();
}
