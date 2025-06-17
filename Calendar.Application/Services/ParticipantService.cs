// Application/Services/ParticipantService.cs
using Calendar.Application.DTOs;
using Calendar.Application.Interfaces;
using Calendar.Domain.Entities;
using Calendar.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Calendar.Application.Services;

public class ParticipantService : IParticipantService
{
    private readonly CalendarDbContext _context;

    public ParticipantService(CalendarDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ParticipantDto>> GetParticipantsByEventAsync(Guid eventId)
    {
        return await _context.Participants
            .Where(p => p.EventId == eventId)
            .Select(p => new ParticipantDto
            {
                Id = p.Id,
                UserId = p.UserId,
                EventId = p.EventId
            })
            .ToListAsync();
    }

    public async Task<ParticipantDto?> GetParticipantAsync(Guid id)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant is null)
        {
            return null;
        }

        return new ParticipantDto
        {
            Id = participant.Id,
            UserId = participant.UserId,
            EventId = participant.EventId
        };
    }

    public async Task<Guid> AddParticipantAsync(Guid eventId, Guid userId)
    {
        var participant = new Participant
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            EventId = eventId
        };

        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();
        return participant.Id;
    }

    public async Task UpdateParticipantAsync(Guid id, ParticipantDto dto)
    {
        var participant = await _context.Participants.FindAsync(id);

        if (participant == null)
        {
            throw new KeyNotFoundException("Participant not found.");
        }

        participant.UserId = dto.UserId;
        participant.EventId = dto.EventId;

        _context.Participants.Update(participant);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveParticipantAsync(Guid id)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant is null)
        {
            return;
        }

        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();
    }
}
