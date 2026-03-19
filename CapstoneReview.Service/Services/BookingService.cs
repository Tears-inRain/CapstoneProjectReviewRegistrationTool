using CapstoneReview.Service.DTOs;
using CapstoneReview.Repository.Entities;
using CapstoneReview.Repository.Interfaces;
using CapstoneReview.Service.Exceptions;
using CapstoneReview.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneReview.Service.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;

    public BookingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<AvailableSlotResponse>> GetAvailableSlotsAsync()
    {
        var slots = await _unitOfWork.Slots.GetAvailableSlotsAsync();
        return slots.Select(s => new AvailableSlotResponse
        {
            SlotId = s.Id,
            Room = s.Room,
            StartTime = s.StartTime,
            EndTime = s.EndTime
        }).ToList();
    }

    public async Task BookTeamAsync(TeamBookingRequest request)
    {
        var team = await _unitOfWork.Teams.GetTeamByIdAsync(request.TeamId);
        if (team == null) throw new BusinessRuleException("Team not found.");
        if (team.LeaderId != request.LeaderId) throw new BusinessRuleException("Only the leader can book slots for the team.");

        var topic = await _unitOfWork.Teams.GetTopicByTeamIdAsync(request.TeamId);
        if (topic == null) throw new BusinessRuleException("Team does not have an assigned topic.");

        var slotTopics = new List<SlotTopic>();
        foreach (var slotId in request.SlotIds)
        {
            var slot = await _unitOfWork.Slots.GetSlotByIdAsync(slotId);
            if (slot == null) throw new BusinessRuleException($"Slot {slotId} not found.");
            if (slot.SlotTopics.Count >= 3) throw new BusinessRuleException($"Slot {slotId} already has maximum 3 topics.");
            
            var alreadyBooked = await _unitOfWork.Slots.HasTopicBookedSlotAsync(topic.Id, slotId);
            if (!alreadyBooked)
            {
                slotTopics.Add(new SlotTopic { SlotId = slotId, TopicId = topic.Id });
            }
        }

        if (slotTopics.Any())
        {
            await _unitOfWork.Slots.AddSlotTopicsAsync(slotTopics);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task BookLecturerAsync(LecturerBookingRequest request)
    {
        var lecturer = await _unitOfWork.Lecturers.GetByIdAsync(request.LecturerId);
        if (lecturer == null) throw new BusinessRuleException("Lecturer not found.");

        var slotLecturers = new List<SlotLecturer>();
        foreach (var slotId in request.SlotIds)
        {
            var slot = await _unitOfWork.Slots.GetSlotByIdAsync(slotId);
            if (slot == null) throw new BusinessRuleException($"Slot {slotId} not found.");
            if (slot.SlotLecturers.Count >= 2) throw new BusinessRuleException($"Slot {slotId} already has maximum 2 reviewers.");

            var alreadyBooked = await _unitOfWork.Slots.HasLecturerBookedSlotAsync(request.LecturerId, slotId);
            if (!alreadyBooked)
            {
                slotLecturers.Add(new SlotLecturer { SlotId = slotId, LecturerId = request.LecturerId });
            }
        }

        if (slotLecturers.Any())
        {
            await _unitOfWork.Slots.AddSlotLecturersAsync(slotLecturers);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task ConfigureLecturerAsync(int lecturerId, ModeratorConfigLecturerRequest request)
    {
        if (request.MinSlot < 0) throw new BusinessRuleException("MinSlot must be >= 0.");
        if (request.MaxSlot < request.MinSlot) throw new BusinessRuleException("MaxSlot must be >= MinSlot.");

        var lecturer = await _unitOfWork.Lecturers.GetByIdAsync(lecturerId);
        if (lecturer == null) throw new BusinessRuleException("Lecturer not found.");

        lecturer.MinSlot = request.MinSlot;
        lecturer.MaxSlot = request.MaxSlot;

        await _unitOfWork.SaveChangesAsync();
    }
}
