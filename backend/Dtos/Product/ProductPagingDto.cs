
namespace backend.Dto.Product;
public class ProductPagingDto
{
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? CategoryName { get; set; }
}