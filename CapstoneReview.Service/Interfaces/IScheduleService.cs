using System.Threading.Tasks;

namespace CapstoneReview.Service.Interfaces;

public interface IScheduleService
{
    // Tự động phân bổ Giảng viên vào các Slot còn trống của 1 Round
    Task AutoScheduleAsync(int reviewRound);

    // Dùng cho Moderator xếp tay / kiểm tra tính hợp lệ trước khi Assign
    Task ValidateLecturerAssignmentAsync(int slotId, int lecturerId);
}
