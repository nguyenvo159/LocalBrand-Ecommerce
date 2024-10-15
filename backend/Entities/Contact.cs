using System;
using backend.Entity;

namespace backend.Entities;

public class Contact
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Guid? UserId { get; set; }

    public virtual User? User { get; set; }
}
