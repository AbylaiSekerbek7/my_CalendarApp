namespace Calendar.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Login { get; set; } = default!;
    public string Name { get; set; } = default!;

    public ICollection<Participant> Participations { get; set; } = new List<Participant>();
}
