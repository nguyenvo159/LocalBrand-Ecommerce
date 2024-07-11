﻿using backend.Dto.Size;

namespace backend.Dto.Product;

public class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public List<string> ImageUrls { get; set; } = new List<string>();
    public List<SizeDto> Sizes { get; set; } = new List<SizeDto>();
}