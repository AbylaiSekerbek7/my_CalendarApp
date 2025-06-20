using Calendar.Application.DTOs;
using Calendar.Application.Interfaces;
using Calendar.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Calendar.Application.Services;

public class AvailabilityService : IAvailabilityService
{
    private readonly CalendarDbContext _context;

    public AvailabilityService(CalendarDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TimeSlot>> FindAvailableTimeSlotsAsync(
        IEnumerable<Guid> userIds,
        DateTime from,
        DateTime to,
        TimeSpan slotDuration)
    {
        var events = await _context.Participants
            .Include(p => p.Event)
            .Where(p => userIds.Contains(p.UserId) &&
                        p.Event.StartTime < to &&
                        p.Event.EndTime > from)
            .Select(p => new
            {
                p.UserId,
                p.Event.StartTime,
                p.Event.EndTime
            })
            .ToListAsync();

        var busyByUser = userIds.ToDictionary(id => id, id => new List<(DateTime, DateTime)>());

        foreach (var e in events)
        {
            busyByUser[e.UserId].Add((e.StartTime, e.EndTime));
        }

        Console.WriteLine("Loaded Events:");
        foreach (var e in events)
        {
            Console.WriteLine($"User: {e.UserId}, Event: {e.StartTime:HH:mm} - {e.EndTime:HH:mm}");
        }


        var workingStart = new TimeSpan(9, 0, 0);
        var workingEnd = new TimeSpan(18, 0, 0);

        var slots = new List<TimeSlot>();
        var currentDay = from.Date;

        while (currentDay <= to.Date)
        {
            var workStartTime = currentDay + workingStart;
            var workEndTime = currentDay + workingEnd;

            var slotStart = workStartTime;
            var slotEnd = slotStart + slotDuration;

            while (slotEnd <= workEndTime && slotEnd <= to)
            {
                bool allFree = userIds.All(userId =>
                    !busyByUser[userId].Any(busy =>
                        slotStart < busy.Item2 && slotEnd > busy.Item1));

                if (allFree)
                {
                    slots.Add(new TimeSlot { Start = slotStart, End = slotEnd });
                    Console.WriteLine($"Slot found: {slotStart:HH:mm} - {slotEnd:HH:mm}");
                }

                slotStart = slotStart.AddMinutes(15);
                slotEnd = slotStart + slotDuration;
            }

            currentDay = currentDay.AddDays(1);
        }

        return slots;
    }
}
