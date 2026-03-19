using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneReview.API.Models.RequestModels;

public class CreateSlotRequest
{
    [Required]
    public int ReviewRound { get; set; } // Ví dụ: 1, 2 hoặc 3

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    [MaxLength(50)]
    public string Room { get; set; } = null!;

    [Required]
    public DateTime RegistrationDeadline { get; set; }
}
