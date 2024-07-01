using System.ComponentModel.DataAnnotations;

namespace backend.Entity;

public record class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
    [StringLength(100, ErrorMessage = "Email length can't be more than 100.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone(ErrorMessage = "Invalid Phone Number format.")]
    [StringLength(20, ErrorMessage = "Phone length can't be more than 20.")]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "PasswordHash length can't be more than 100.")]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [StringLength(50, ErrorMessage = "Role length can't be more than 50.")]
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Cart Cart { get; set; } = new Cart();
    public List<Order> Orders { get; set; } = new List<Order>();
    public List<Review> Reviews { get; set; } = new List<Review>();
}
