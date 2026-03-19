using System.Linq;
using System.Threading.Tasks;
using CapstoneReview.Repository.Entities;
using CapstoneReview.Repository.Interfaces;
using CapstoneReview.Service.Exceptions;
using CapstoneReview.Service.Interfaces;

namespace CapstoneReview.Service.Services;

public class ScheduleService : IScheduleService
{
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AutoScheduleAsync(int reviewRound)
    {
        // 1. Lấy danh sách slot và lecturer từ Repository
        var slots = await _unitOfWork.Slots.GetSlotsByRoundWithDetailsAsync(reviewRound);
        var lecturers = await _unitOfWork.Lecturers.GetAllLecturersWithDetailsAsync();

        // 2. Thuật toán Greedy
        foreach (var slot in slots)
        {
            var neededReviewers = 2 - slot.SlotLecturers.Count;
            for (int i = 0; i < neededReviewers; i++)
            {
                var validLecturer = lecturers
                    .Where(l => l.SlotLecturers.Count < l.MaxSlot)
                    .FirstOrDefault(l => 
                        !slot.SlotTopics.Any(st => st.Topic.LecturerId == l.Id) && 
                        !slot.SlotLecturers.Any(sl => sl.LecturerId == l.Id)
                    );

                if (validLecturer != null)
                {
                    var slotLecturer = new SlotLecturer { SlotId = slot.Id, LecturerId = validLecturer.Id };
                    
                    // Gọi qua repo thay vì set thẳng vào DbSet
                    _unitOfWork.Slots.AddSlotLecturer(slotLecturer);
                    
                    slot.SlotLecturers.Add(slotLecturer);
                    validLecturer.SlotLecturers.Add(slotLecturer);
                }
                else
                {
                    throw new BusinessRuleException(
                        $"Không thể tìm đủ Giảng viên phù hợp cho Slot {slot.Id} (Round {reviewRound}). " +
                        "Nguyên nhân có thể do kẹt conflict GVHD, hoặc toàn bộ GV đều đã đạt Max Slot."
                    );
                }
            }
        }

        // 3. Nghiệm thu MinSlot
        var missingMinSlotLecturers = lecturers.Where(l => l.SlotLecturers.Count < l.MinSlot).ToList();
        if (missingMinSlotLecturers.Any())
        {
            var names = string.Join(", ", missingMinSlotLecturers.Select(l => l.FullName));
            throw new BusinessRuleException(
                $"Thuật toán đã xếp xong lịch nhưng phát hiện vi phạm quy tắc: " +
                $"Các Giảng viên sau không đạt đủ yêu cầu Min Slot: {names}. " +
                $"Yêu cầu Moderator giảm Min Slot hoặc tăng thêm đề tài cho Round này."
            );
        }

        // Lưa toàn bộ thay đổi thông qua UnitOfWork
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ValidateLecturerAssignmentAsync(int slotId, int lecturerId)
    {
        var slot = await _unitOfWork.Slots.GetSlotByIdWithDetailsAsync(slotId);
        if (slot == null) throw new BusinessRuleException("Slot không tồn tại.");

        var lecturer = await _unitOfWork.Lecturers.GetLecturerByIdWithDetailsAsync(lecturerId);
        if (lecturer == null) throw new BusinessRuleException("Giảng viên không tồn tại.");

        bool isSupervisor = slot.SlotTopics.Any(st => st.Topic.LecturerId == lecturerId);
        if (isSupervisor)
            throw new BusinessRuleException("Lecturer KHÔNG được phép tham gia review đề tài mà mình đang hướng dẫn.");

        if (lecturer.SlotLecturers.Count >= lecturer.MaxSlot)
            throw new BusinessRuleException($"Lecturer {lecturer.FullName} đã đạt mức tối đa {lecturer.MaxSlot} slot review.");

        if (slot.SlotLecturers.Count >= 2)
            throw new BusinessRuleException($"Slot {slot.Id} đã đủ 2 Giảng viên Review.");
            
        if (slot.SlotLecturers.Any(sl => sl.LecturerId == lecturerId))
            throw new BusinessRuleException("Giảng viên này đã được xếp vào slot này từ trước.");
    }
}
