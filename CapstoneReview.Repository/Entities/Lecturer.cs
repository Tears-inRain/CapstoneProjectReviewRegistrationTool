using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapstoneReview.Repository.Entities;

public class Lecturer
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    
    public int MinSlot { get; set; } = 2; // Default
    public int MaxSlot { get; set; } = 5; // Default

    // Navigation properties
    public ICollection<Topic> InstructedTopics { get; set; } = new List<Topic>();
    public ICollection<SlotLecturer> SlotLecturers { get; set; } = new List<SlotLecturer>();
}
