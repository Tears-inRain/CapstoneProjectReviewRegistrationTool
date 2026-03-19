using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CapstoneReview.API.Models.RequestModels;
using CapstoneReview.API.Models.ResponseModels;
using CapstoneReview.Service.Interfaces;
using CapstoneReview.Service.Exceptions;
using CapstoneReview.Repository.Interfaces;

namespace CapstoneReview.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleController(IScheduleService scheduleService, IUnitOfWork unitOfWork)
    {
        _scheduleService = scheduleService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("auto-schedule")]
    public async Task<IActionResult> AutoSchedule([FromBody] AutoScheduleRequest request)
    {
        try
        {
            // Thực thi thuật toán xếp lịch (Business Logic Layer)
            await _scheduleService.AutoScheduleAsync(request.ReviewRound);
            return Ok(new { message = $"Đã tự động xếp lịch thành công cho Review Round {request.ReviewRound}." });
        }
        catch (BusinessRuleException ex)
        {
            // Bắt lỗi nghiệp vụ -> Return 400 Bad Request
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Lỗi hệ thống -> Return 500 Internal Server Error
            return StatusCode(500, new { error = "Lỗi hệ thống trong quá trình xếp lịch.", details = ex.Message });
        }
    }

    [HttpPost("validate-assignment")]
    public async Task<IActionResult> ValidateManualAssignment(int slotId, int lecturerId)
    {
        try
        {
            await _scheduleService.ValidateLecturerAssignmentAsync(slotId, lecturerId);
            return Ok(new { message = "Gán giảng viên hợp lệ! Không có conflict." });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Lỗi hệ thống.", details = ex.Message });
        }
    }

    [HttpGet("{reviewRound}")]
    public async Task<IActionResult> GetSchedules(int reviewRound)
    {
        try
        {
            // Lấy dữ liệu qua Repo (không xử lý nghiệp vụ, chỉ query thuần tuý)
            var slots = await _unitOfWork.Slots.GetSlotsByRoundWithDetailsAsync(reviewRound);

            // Tách biệt Entity và DTO (Mapping trả về cho Frontend)
            var response = slots.Select(s => new ScheduleResponse
            {
                SlotId = s.Id,
                ReviewRound = s.ReviewRound,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Room = s.Room,
                Reviewers = s.SlotLecturers.Select(sl => new LecturerDto
                {
                    LecturerId = sl.Lecturer?.Id ?? 0,
                    FullName = sl.Lecturer?.FullName ?? "Unknown",
                    Email = sl.Lecturer?.Email ?? ""
                }).ToList(),
                Topics = s.SlotTopics.Select(st => new TopicDto
                {
                    TopicId = st.Topic?.Id ?? 0,
                    Title = st.Topic?.Title ?? "Unknown Focus",
                    TeamId = st.Topic?.Team?.Id ?? 0,
                    TeamName = st.Topic?.Team?.TeamName ?? "N/A", // Assume we might need to include Team in Repo later
                    InstructorId = st.Topic?.LecturerId ?? 0,
                    InstructorName = "Liên kết GVHD" // Đã có Instructor ID, FE sẽ map tên qua cache hoặc Repo phải fetch thêm Include
                }).ToList()
            }).ToList();

            return Ok(response); // Trả về 200 OK
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Không thể lấy danh sách lịch.", details = ex.Message });
        }
    }
}
