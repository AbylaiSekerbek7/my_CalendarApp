// DTOs/ParticipantDto.cs
namespace Calendar.Application.DTOs;

public class ParticipantDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
}
