namespace Calendar.Domain.Entities;

public class Participant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid EventId { get; set; }
    public CalendarEvent Event { get; set; } = default!;
}
