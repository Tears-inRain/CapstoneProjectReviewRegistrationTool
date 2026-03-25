using CapstoneReview.Service.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapstoneReview.Service.Interfaces;

public interface IBookingService
{
    Task<List<AvailableSlotResponse>> GetAvailableSlotsAsync();
    Task BookTeamAsync(TeamBookingRequest request);
    Task BookLecturerAsync(LecturerBookingRequest request);
    Task ConfigureLecturerAsync(int lecturerId, ModeratorConfigLecturerRequest request);
    Task CreateSlotAsync(CreateSlotDto request);
}
