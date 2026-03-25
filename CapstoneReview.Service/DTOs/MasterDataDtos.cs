namespace CapstoneReview.Service.DTOs;

public class MasterLecturerDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int MinSlot { get; set; }
    public int MaxSlot { get; set; }
}

public class MasterTeamDto
{
    public int Id { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int LeaderId { get; set; }
    public int TopicId { get; set; }
}
