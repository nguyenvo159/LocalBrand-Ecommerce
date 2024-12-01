using System;

namespace backend.Dto.Product;

public class ProductSpecialUpdate
{
    public List<Guid> ProductIds { get; set; }
    public int Percentage { get; set; }
}
