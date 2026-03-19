using System.ComponentModel.DataAnnotations;

namespace CapstoneReview.API.Models.RequestModels;

public class AutoScheduleRequest
{
    [Required]
    public int ReviewRound { get; set; }
    
    // Có thể bổ sung thêm các parameter khác như Mode (Strict/Flexible) nếu thuật toán cần
}
