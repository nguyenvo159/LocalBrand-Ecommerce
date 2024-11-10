using System.ComponentModel.DataAnnotations;
using backend.Text.Enums;

namespace backend.Dto.Order;


public class OrderUpdateDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string UserPhone { get; set; } = string.Empty;
    [Required]
    public Enums.OrderStatus Status { get; set; }
    public Enums.PayType? PayType { get; set; }
}
