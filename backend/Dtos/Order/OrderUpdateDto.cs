using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Order;


public class OrderUpdateDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string UserPhone { get; set; } = string.Empty;
    [Required]
    public string UserEmail { get; set; } = string.Empty;
    [Required]
    public string Address { get; set; } = string.Empty;
    [Required]
    public string Status { get; set; } = string.Empty;
}
