using System.Threading.Tasks;
using CapstoneReview.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneReview.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly IMasterDataService _masterDataService;

    public TeamController(IMasterDataService masterDataService)
    {
        _masterDataService = masterDataService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeams()
    {
        try
        {
            var teams = await _masterDataService.GetAllTeamsAsync();
            return Ok(teams);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { error = "Không thể lấy danh sách nhóm", details = ex.Message });
        }
    }
}
