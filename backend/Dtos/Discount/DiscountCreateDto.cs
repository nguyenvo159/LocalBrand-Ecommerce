using System.ComponentModel.DataAnnotations;

namespace backend;

public class DiscountCreateDto
{
    [Required]
    public int Amount { get; set; }
    [Range(1000, 10000000, ErrorMessage = "Require money must be between 1000 and 10000000")]
    public decimal RequireMoney { get; set; }
    [Range(1, 100, ErrorMessage = "Discount percentage must be between 1 and 100")]
    public decimal DiscountPercentage { get; set; }
    [Range(1000, 10000000, ErrorMessage = "Maximum discount must be between 1000 and 10000000")]
    public decimal MaximumDiscount { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.Now.AddDays(30);
}
