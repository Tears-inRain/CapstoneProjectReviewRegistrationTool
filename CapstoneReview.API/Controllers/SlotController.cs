using CapstoneReview.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneReview.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SlotController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public SlotController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableSlots()
    {
        try
        {
            var slots = await _bookingService.GetAvailableSlotsAsync();
            return Ok(slots);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSlot([FromBody] CapstoneReview.API.Models.RequestModels.CreateSlotRequest request)
    {
        try
        {
            var dto = new CapstoneReview.Service.DTOs.CreateSlotDto
            {
                ReviewRound = request.ReviewRound,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Room = request.Room,
                RegistrationDeadline = request.RegistrationDeadline
            };

            await _bookingService.CreateSlotAsync(dto);
            return Ok(new { message = "Slot created successfully." });
        }
        catch (CapstoneReview.Service.Exceptions.BusinessRuleException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { error = "Lỗi hệ thống", details = ex.Message });
        }
    }
}
