using Calendar.Domain.Entities;
using Calendar.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Calendar.WebApi.Controllers;

[ApiController]
[Route("api/v1/events")]
public class EventsController : ControllerBase
{
    private readonly CalendarDbContext _context;

    public EventsController(CalendarDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetEvents()
    {
        return Ok(await _context.Events
            .Include(e => e.Participants)
            .ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CalendarEvent>> GetEvent(Guid id)
    {
        var calendarEvent = await _context.Events
            .Include(e => e.Participants)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (calendarEvent is null)
        {
            return NotFound();
        }

        return Ok(calendarEvent);
    }

    [HttpPost]
    public async Task<ActionResult<CalendarEvent>> CreateEvent(CalendarEvent calendarEvent)
    {
        _context.Events.Add(calendarEvent);
        await _context.SaveChangesAsync();
        return Ok(calendarEvent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, CalendarEvent updated)
    {
        var existing = await _context.Events.FindAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Title = updated.Title;
        existing.Description = updated.Description;
        existing.StartTime = updated.StartTime;
        existing.EndTime = updated.EndTime;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var calendarEvent = await _context.Events.FindAsync(id);
        if (calendarEvent is null)
        {
            return NotFound();
        }

        _context.Events.Remove(calendarEvent);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
