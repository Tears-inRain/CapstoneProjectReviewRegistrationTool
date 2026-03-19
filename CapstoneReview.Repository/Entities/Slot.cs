using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapstoneReview.Repository.Entities;

public class Slot
{
    public int Id { get; set; }
    
    public int ReviewRound { get; set; } // e.g. 1, 2, 3

    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }

    [Required]
    [MaxLength(50)]
    public string Room { get; set; } = null!;

    // Deadline for registration
    public DateTime RegistrationDeadline { get; set; }

    // Navigation properties
    public ICollection<SlotLecturer> SlotLecturers { get; set; } = new List<SlotLecturer>();
    public ICollection<SlotTopic> SlotTopics { get; set; } = new List<SlotTopic>();
}
