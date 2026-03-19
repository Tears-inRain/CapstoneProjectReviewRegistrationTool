using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapstoneReview.Repository.Entities;

public class Team
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string TeamName { get; set; } = null!;

    public int LeaderId { get; set; }

    public int TopicId { get; set; }
    public Topic Topic { get; set; } = null!;

    // Navigation properties
    public ICollection<Student> Students { get; set; } = new List<Student>();
}
