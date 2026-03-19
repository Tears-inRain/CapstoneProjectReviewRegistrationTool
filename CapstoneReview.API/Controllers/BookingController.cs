using CapstoneReview.Service.DTOs;
using CapstoneReview.Service.Exceptions;
using CapstoneReview.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneReview.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("team")]
    public async Task<IActionResult> BookTeam([FromBody] TeamBookingRequest request)
    {
        try
        {
            await _bookingService.BookTeamAsync(request);
            return Ok(new { message = "Team booked successfully." });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("lecturer")]
    public async Task<IActionResult> BookLecturer([FromBody] LecturerBookingRequest request)
    {
        try
        {
            await _bookingService.BookLecturerAsync(request);
            return Ok(new { message = "Lecturer booked successfully." });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
