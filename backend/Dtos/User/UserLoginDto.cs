using System.ComponentModel.DataAnnotations;

namespace backend.Dto.User;

public class UserLoginDto
{
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;
}
