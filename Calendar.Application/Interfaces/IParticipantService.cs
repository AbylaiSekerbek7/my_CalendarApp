using Calendar.Application.DTOs;

namespace Calendar.Application.Interfaces;

public interface IParticipantService
{
    Task<IEnumerable<ParticipantDto>> GetParticipantsByEventAsync(Guid eventId);
    Task<ParticipantDto?> GetParticipantAsync(Guid id);
    Task<Guid> AddParticipantAsync(Guid eventId, Guid userId);
    Task UpdateParticipantAsync(Guid id, ParticipantDto dto);
    Task RemoveParticipantAsync(Guid id);
}
