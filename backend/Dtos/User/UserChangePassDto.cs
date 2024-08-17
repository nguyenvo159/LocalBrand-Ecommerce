using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.User;

public class UserChangePassDto
{
    [Required]
    [MinLength(6, ErrorMessage = "Password must be between 6 and 20 characters")]
    [MaxLength(20, ErrorMessage = "Password must be between 6 and 20 characters")]
    public string OldPassword { get; set; }
    [Required]
    [MinLength(6, ErrorMessage = "Password must be between 6 and 20 characters")]
    [MaxLength(20, ErrorMessage = "Password must be between 6 and 20 characters")]
    public string NewPassword { get; set; }

}
