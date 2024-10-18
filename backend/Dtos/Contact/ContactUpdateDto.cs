using System;

namespace backend.Dtos.Contact;

public class ContactUpdateDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string? Reply { get; set; }
}
