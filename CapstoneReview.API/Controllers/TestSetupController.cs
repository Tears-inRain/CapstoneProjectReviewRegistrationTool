using CapstoneReview.Repository.Data.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneReview.API.Controllers;

[ApiController]
[Route("api/test-setup")]
public class TestSetupController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TestSetupController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("reset-db")]
    public async Task<IActionResult> ResetDatabase()
    {
        var scriptPaths = new[] { "init_db_mock.sql", "../init_db_mock.sql", "/app/init_db_mock.sql" };
        var scriptPath = scriptPaths.FirstOrDefault(System.IO.File.Exists);

        if (scriptPath == null)
        {
            return NotFound(new { message = "Không tìm thấy file init_db_mock.sql" });
        }

        try
        {
            // Đảm bảo CSDL được tạo ra theo Model của EF Core nếu chưa tồn tại
            await _context.Database.EnsureCreatedAsync();

            var script = await System.IO.File.ReadAllTextAsync(scriptPath);
            await _context.Database.ExecuteSqlRawAsync(script);
            return Ok(new { message = "Database wiped and mock data inserted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Database reset failed.", details = ex.Message });
        }
    }
}
