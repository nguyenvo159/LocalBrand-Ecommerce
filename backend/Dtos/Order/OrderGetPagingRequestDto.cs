using System;
using backend.Text.Enums;

namespace backend.Dtos.Order;

public class OrderGetPagingRequestDto
{
    public string? Search { get; set; }
    public Enums.OrderStatus? Status { get; set; }
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 100;

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

}
