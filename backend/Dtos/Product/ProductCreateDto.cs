using System.ComponentModel.DataAnnotations;
using backend.Dto.Size;

namespace backend.Dto.Product;

public class ProductCreateDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    public List<string> ImageUrl { get; set; } = new List<string>();
    public List<SizeDto> Sizes { get; set; } = new List<SizeDto>();
}
