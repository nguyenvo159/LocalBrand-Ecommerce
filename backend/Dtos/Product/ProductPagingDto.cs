using backend.Text.Enums;

namespace backend.Dto.Product;
public class ProductPagingDto
{
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? CategoryName { get; set; }
    public Enums.SortBy? SortBy { get; set; }
    public Enums.OrderByPrice? OrderByPrice { get; set; }
    public int? Special { get; set; }
}