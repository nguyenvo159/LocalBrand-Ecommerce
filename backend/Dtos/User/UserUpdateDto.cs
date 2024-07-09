using System.ComponentModel.DataAnnotations;

namespace backend.Dto.User;

public class UserUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Password { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Role { get; set; } = string.Empty;

}
