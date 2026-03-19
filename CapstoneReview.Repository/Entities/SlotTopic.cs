namespace CapstoneReview.Repository.Entities;

public class SlotTopic
{
    public int SlotId { get; set; }
    public Slot Slot { get; set; } = null!;

    public int TopicId { get; set; }
    public Topic Topic { get; set; } = null!;
}
