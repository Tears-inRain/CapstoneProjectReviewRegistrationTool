using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapstoneReview.Repository.Entities;

public class Topic
{
    public int Id { get; set; }

    [Required]
    [MaxLength(250)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    public int LecturerId { get; set; } 
    public Lecturer Lecturer { get; set; } = null!;

    public Team? Team { get; set; } 

    public ICollection<SlotTopic> SlotTopics { get; set; } = new List<SlotTopic>();
}
