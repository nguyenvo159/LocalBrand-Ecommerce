using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Category;

public class CategoryCreateUpdateDto
{
    public Guid? Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
}
