using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Contact;

public class ContactCreateDto
{
    public string Name { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
