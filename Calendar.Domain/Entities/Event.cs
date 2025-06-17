namespace Calendar.Domain.Entities;

public class CalendarEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
}
