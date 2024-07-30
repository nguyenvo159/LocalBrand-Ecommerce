﻿namespace backend.Entity;

public record class Cart
{
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public virtual User? User { get; set; }
    public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>();
}
