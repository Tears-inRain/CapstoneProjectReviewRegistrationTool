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
}
