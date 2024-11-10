using System.ComponentModel.DataAnnotations;
using backend.Text.Enums;

namespace backend.Dto.Order;

public class OrderCreateDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string UserPhone { get; set; } = string.Empty;

    [Required]
    public string UserEmail { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;
    [Required]
    public Enums.ShipType ShipType { get; set; } = Enums.ShipType.Standard;
    public Enums.PayType PayType { get; set; } = Enums.PayType.COD;

    public string? Note { get; set; } = string.Empty;

    public string? Code { get; set; } = string.Empty;
}
