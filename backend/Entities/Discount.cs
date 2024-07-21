namespace backend.Entity;

public record class Discount
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal RequireMoney { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal MaximumDiscount { get; set; }
    public DateTime ExpiryDate { get; set; }
}
