using Calendar.Application.DTOs;
using Calendar.Application.Interfaces;
using Calendar.Application.Services;
using Calendar.Domain.Entities;
using Calendar.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Calendar.Tests;

public class AvailabilityServiceTests
{
    private CalendarDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<CalendarDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new CalendarDbContext(options);

        // Seed данные
        var user1 = new User { Id = Guid.NewGuid(), Login = "john", Name = "John" };
        var user2 = new User { Id = Guid.NewGuid(), Login = "jane", Name = "Jane" };

        var busyEvent1 = new CalendarEvent
        {
            Id = Guid.NewGuid(),
            Title = "Busy",
            StartTime = new DateTime(2025, 6, 19, 10, 0, 0),
            EndTime = new DateTime(2025, 6, 19, 11, 0, 0),
        };

        var busyEvent2 = new CalendarEvent
        {
            Id = Guid.NewGuid(),
            Title = "Also Busy",
            StartTime = new DateTime(2025, 6, 19, 13, 0, 0),
            EndTime = new DateTime(2025, 6, 19, 14, 0, 0),
        };

        var participant1 = new Participant { Id = Guid.NewGuid(), User = user1, Event = busyEvent1 };
        var participant2 = new Participant { Id = Guid.NewGuid(), User = user2, Event = busyEvent1 };

        var participant3 = new Participant { Id = Guid.NewGuid(), User = user1, Event = busyEvent2 };
        var participant4 = new Participant { Id = Guid.NewGuid(), User = user2, Event = busyEvent2 };

        context.Users.AddRange(user1, user2);
        context.Events.AddRange(busyEvent1, busyEvent2);
        context.Participants.AddRange(participant1, participant2, participant3, participant4);

        context.SaveChanges();
        return context;
    }


    [Fact]
    public async Task ShouldReturnFreeSlotsWhenUsersAreFree()
    {
        // Arrange
        var context = GetDbContext();
        var service = new AvailabilityService(context);

        var from = new DateTime(2025, 6, 19, 9, 0, 0);
        var to = new DateTime(2025, 6, 19, 13, 0, 0);
        var userIds = context.Users.Select(u => u.Id).ToList();
        var duration = TimeSpan.FromMinutes(30);

        // Act
        var slots = await service.FindAvailableTimeSlotsAsync(userIds, from, to, duration);

        // Output для отладки
        foreach (var slot in slots)
        {
            Console.WriteLine($"Slot: {slot.Start:HH:mm} - {slot.End:HH:mm}");
        }

        // Assert
        Assert.NotEmpty(slots);

        // Временные промежутки, которые заняты (по событиям)
        var busyTimes = new List<(TimeSpan start, TimeSpan end)>
        {
            (new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0))
        };

        Assert.All(slots, slot =>
        {
            Assert.InRange(slot.Start, from, to);
            Assert.InRange(slot.End, from, to);
            Assert.Equal(duration, slot.End - slot.Start);

            // Проверка, что слот не пересекается с занятыми промежутками
            foreach (var (busyStart, busyEnd) in busyTimes)
            {
                bool overlaps = slot.Start.TimeOfDay < busyEnd && slot.End.TimeOfDay > busyStart;
                Assert.False(overlaps, $"Conflict: {slot.Start:HH:mm} - {slot.End:HH:mm} overlaps with busy time {busyStart}-{busyEnd}");
            }
        });
    }

}
