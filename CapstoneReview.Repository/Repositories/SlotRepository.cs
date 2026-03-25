using CapstoneReview.Repository.Data.DbContexts;
using CapstoneReview.Repository.Entities;
using CapstoneReview.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneReview.Repository.Repositories;

public class SlotRepository : ISlotRepository
{
    private readonly ApplicationDbContext _context;

    public SlotRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Slot>> GetSlotsByRoundWithDetailsAsync(int reviewRound)
    {
        return await _context.Slots
            .Where(s => s.ReviewRound == reviewRound)
            .Include(s => s.SlotLecturers)
                .ThenInclude(sl => sl.Lecturer)
            .Include(s => s.SlotTopics)
                .ThenInclude(st => st.Topic)
                    .ThenInclude(t => t.Team)
            .ToListAsync();
    }

    public async Task<Slot?> GetSlotByIdWithDetailsAsync(int slotId)
    {
        return await _context.Slots
            .Include(s => s.SlotLecturers)
                .ThenInclude(sl => sl.Lecturer)
            .Include(s => s.SlotTopics)
                .ThenInclude(st => st.Topic)
            .FirstOrDefaultAsync(s => s.Id == slotId);
    }

    public void AddSlotLecturer(SlotLecturer slotLecturer)
    {
        _context.SlotLecturers.Add(slotLecturer);
    }

    public void AddSlot(Slot slot)
    {
        _context.Slots.Add(slot);
    }

    public Task<Slot?> GetSlotByIdAsync(int slotId)
    {
        return _context.Slots
            .Include(s => s.SlotTopics)
            .Include(s => s.SlotLecturers)
            .FirstOrDefaultAsync(s => s.Id == slotId);
    }

    public Task<List<Slot>> GetAvailableSlotsAsync()
    {
        return _context.Slots
            .Where(s => s.SlotTopics.Count < 3 || s.SlotLecturers.Count < 2)
            .ToListAsync();
    }

    public async Task AddSlotTopicsAsync(List<SlotTopic> slotTopics)
    {
        await _context.SlotTopics.AddRangeAsync(slotTopics);
    }

    public async Task AddSlotLecturersAsync(List<SlotLecturer> slotLecturers)
    {
        await _context.SlotLecturers.AddRangeAsync(slotLecturers);
    }

    public Task<bool> HasLecturerBookedSlotAsync(int lecturerId, int slotId)
    {
        return _context.SlotLecturers.AnyAsync(sl => sl.LecturerId == lecturerId && sl.SlotId == slotId);
    }

    public Task<bool> HasTopicBookedSlotAsync(int topicId, int slotId)
    {
        return _context.SlotTopics.AnyAsync(st => st.TopicId == topicId && st.SlotId == slotId);
    }
}
