using CapstoneReview.Service.DTOs;
using CapstoneReview.Service.Exceptions;
using CapstoneReview.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneReview.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModeratorController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public ModeratorController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPut("lecturer-config/{lecturerId}")]
    public async Task<IActionResult> ConfigLecturer(int lecturerId, [FromBody] ModeratorConfigLecturerRequest request)
    {
        try
        {
            await _bookingService.ConfigureLecturerAsync(lecturerId, request);
            return Ok(new { message = "Lecturer configured successfully." });
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
