using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneReview.Repository.Entities;

namespace CapstoneReview.Repository.Interfaces;

public interface ISlotRepository
{
    Task<List<Slot>> GetSlotsByRoundWithDetailsAsync(int reviewRound);
    Task<Slot?> GetSlotByIdWithDetailsAsync(int slotId);
    void AddSlotLecturer(SlotLecturer slotLecturer);
    void AddSlot(Slot slot);
    Task<Slot?> GetSlotByIdAsync(int slotId);
    Task<List<Slot>> GetAvailableSlotsAsync();
    Task AddSlotTopicsAsync(List<SlotTopic> slotTopics);
    Task AddSlotLecturersAsync(List<SlotLecturer> slotLecturers);
    Task<bool> HasLecturerBookedSlotAsync(int lecturerId, int slotId);
    Task<bool> HasTopicBookedSlotAsync(int topicId, int slotId);
}
