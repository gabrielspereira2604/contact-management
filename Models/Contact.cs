using System.ComponentModel.DataAnnotations;

namespace ContactManagement.Models;

public class Contact
{
    public int Id { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Name must be greater than 5 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "Contact must be exactly 9 digits.")]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
