﻿using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Cart;

public class CartItemCreateDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public string SizeName { get; set; } = string.Empty;

    [Range(1, 99, ErrorMessage = "Quantity must be 1 to 99")]
    public int Quantity { get; set; }
}
