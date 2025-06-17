using Calendar.Application.DTOs;
using Calendar.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.WebApi.Controllers;

[ApiController]
[Route("api/v1/availability")]
public class AvailabilityController : ControllerBase
{
    private readonly IAvailabilityService _service;

    public AvailabilityController(IAvailabilityService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableSlots(
        [FromQuery] List<Guid> userIds,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] int slotMinutes = 30)
    {
        var slots = await _service.FindAvailableTimeSlotsAsync(userIds, from, to, TimeSpan.FromMinutes(slotMinutes));
        return Ok(slots);
    }
}
