namespace CapstoneReview.Repository.Entities;

public class SlotLecturer
{
    public int SlotId { get; set; }
    public Slot Slot { get; set; } = null!;

    public int LecturerId { get; set; }
    public Lecturer Lecturer { get; set; } = null!;
}
