using System;

namespace backend.Dtos.Product;

public class ProductAnalytics
{
    public int TotalProduct { get; set; }
    public int TotalProductSold { get; set; }
    public int TotalProductSoldForWeek { get; set; }
    public int TotalProductSoldForMonth { get; set; }
}
