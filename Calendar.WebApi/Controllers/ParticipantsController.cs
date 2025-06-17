using Calendar.Application.DTOs;
using Calendar.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.WebApi.Controllers;

[ApiController]
[Route("api/v1/events/{eventId:guid}/participants")]
public class ParticipantsController : ControllerBase
{
    private readonly IParticipantService _participantService;

    public ParticipantsController(IParticipantService participantService)
    {
        _participantService = participantService;
    }

    // GET /api/v1/events/{eventId}/participants
    [HttpGet]
    public async Task<IActionResult> GetParticipants(Guid eventId)
    {
        var participants = await _participantService.GetParticipantsByEventAsync(eventId);
        return Ok(participants);
    }

    // POST /api/v1/events/{eventId}/participants?userId={userId}
    [HttpPost]
    public async Task<IActionResult> AddParticipant(Guid eventId, [FromQuery] Guid userId)
    {
        var id = await _participantService.AddParticipantAsync(eventId, userId);
        return CreatedAtAction(nameof(GetParticipant), new { eventId, id }, new { id });
    }

    // GET /api/v1/events/{eventId}/participants/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetParticipant(Guid eventId, Guid id)
    {
        var participant = await _participantService.GetParticipantAsync(id);
        if (participant == null)
        {
            return NotFound();
        }

        return Ok(participant);
    }

    // PUT /api/v1/events/{eventId}/participants/{id}
    [HttpPut("{id:guid}")]
#pragma warning disable IDE0060 // Remove unused parameter
    public async Task<IActionResult> UpdateParticipant(Guid eventId, Guid id, [FromBody] ParticipantDto dto)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        if (id != dto.Id)
        {
            return BadRequest("Mismatched ID");
        }

        await _participantService.UpdateParticipantAsync(id, dto);
        return NoContent();
    }

    // DELETE /api/v1/events/{eventId}/participants/{id}
    [HttpDelete("{id:guid}")]
#pragma warning disable IDE0060 // Remove unused parameter
    public async Task<IActionResult> RemoveParticipant(Guid eventId, Guid id)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        await _participantService.RemoveParticipantAsync(id);
        return NoContent();
    }
}
