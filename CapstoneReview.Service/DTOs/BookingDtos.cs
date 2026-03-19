namespace CapstoneReview.Service.DTOs;

public class AvailableSlotResponse
{
    public int SlotId { get; set; }
    public System.DateTime StartTime { get; set; }
    public System.DateTime EndTime { get; set; }
    public string Room { get; set; } = string.Empty;
}

public class TeamBookingRequest
{
    public int TeamId { get; set; }
    public int LeaderId { get; set; }
    public System.Collections.Generic.List<int> SlotIds { get; set; } = new();
}

public class LecturerBookingRequest
{
    public int LecturerId { get; set; }
    public System.Collections.Generic.List<int> SlotIds { get; set; } = new();
}

public class ModeratorConfigLecturerRequest
{
    public int MinSlot { get; set; }
    public int MaxSlot { get; set; }
}
