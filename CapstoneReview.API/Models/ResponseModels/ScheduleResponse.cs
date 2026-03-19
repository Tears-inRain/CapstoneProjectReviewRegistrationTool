using System;
using System.Collections.Generic;

namespace CapstoneReview.API.Models.ResponseModels;

public class ScheduleResponse
{
    public int SlotId { get; set; }
    public int ReviewRound { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Room { get; set; } = null!;

    // Danh sách Giảng viên Review (tối đa 2)
    public List<LecturerDto> Reviewers { get; set; } = new List<LecturerDto>();

    // Danh sách Đề tài được review trong slot này (tối đa 3)
    public List<TopicDto> Topics { get; set; } = new List<TopicDto>();
}

public class LecturerDto
{
    public int LecturerId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class TopicDto
{
    public int TopicId { get; set; }
    public string Title { get; set; } = null!;

    // Thông tin Nhóm
    public int TeamId { get; set; }
    public string TeamName { get; set; } = null!;

    // Thông tin Giảng viên Hướng dẫn (để React dễ hiển thị)
    public int InstructorId { get; set; }
    public string InstructorName { get; set; } = null!;
}
