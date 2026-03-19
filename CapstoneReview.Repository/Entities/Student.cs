using System.ComponentModel.DataAnnotations;

namespace CapstoneReview.Repository.Entities;

public class Student
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string RollNumber { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    public int? TeamId { get; set; }
    public Team? Team { get; set; }
}
