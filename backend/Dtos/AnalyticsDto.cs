using System;
using backend.Dtos.Category;

namespace backend.Dto.Common;

public class AnalyticsDto
{
    public int TotalOrders { get; set; }
    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<CategoryCountDto> Categories { get; set; }
    public List<RevenueDto> Revenues { get; set; }


}

public class CategoryCountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Count { get; set; }
    public float Percentage { get; set; }
}

public class RevenueDto
{
    public int Day { get; set; }
    public int OrderCount { get; set; }
    public decimal Revenue { get; set; }
}
