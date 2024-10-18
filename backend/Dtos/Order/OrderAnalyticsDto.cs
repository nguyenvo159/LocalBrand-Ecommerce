using System;

namespace backend.Dtos.Order;

public class OrderAnalyticsDto
{
    public int OrderCount { get; set; }
    public int OrderCountLastWeek { get; set; }
    public decimal RevenuesWeek { get; set; }
    public decimal RevenuesLastWeek { get; set; }
    public decimal RevenueMonth { get; set; }
    public decimal RevenueLastMonth { get; set; }
    public int CanceledOrdersThisWeek { get; set; }
    public int CanceledOrdersLastWeek { get; set; }
}
