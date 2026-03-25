using System.Threading.Tasks;
using CapstoneReview.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneReview.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LecturerController : ControllerBase
{
    private readonly IMasterDataService _masterDataService;

    public LecturerController(IMasterDataService masterDataService)
    {
        _masterDataService = masterDataService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLecturers()
    {
        try
        {
            var lecturers = await _masterDataService.GetAllLecturersAsync();
            return Ok(lecturers);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { error = "Không thể lấy danh sách giảng viên", details = ex.Message });
        }
    }
}
