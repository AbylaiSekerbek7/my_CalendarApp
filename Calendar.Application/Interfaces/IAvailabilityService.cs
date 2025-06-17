using Calendar.Application.DTOs;

namespace Calendar.Application.Interfaces;

public interface IAvailabilityService
{
    Task<IEnumerable<TimeSlot>> FindAvailableTimeSlotsAsync(
        IEnumerable<Guid> userIds,
        DateTime from,
#pragma warning disable CA1716 // Identifiers should not match keywords
        DateTime to,
#pragma warning restore CA1716 // Identifiers should not match keywords
        TimeSpan slotDuration);
}
