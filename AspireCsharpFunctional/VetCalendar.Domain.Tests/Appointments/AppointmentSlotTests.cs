using VetCalendar.Domain.Appointments;

namespace VetCalendar.Domain.Tests.Appointments;

public class AppointmentSlotTests
{
    [Fact]
    public void Create_ValidData_ReturnsSuccess()
    {
        var date      = new DateOnly(2025, 1, 10);
        var startTime = new TimeOnly(9, 0);
        var duration  = TimeSpan.FromMinutes(30);

        var result = AppointmentSlot.Create(date, startTime, duration);

        Assert.True(result.IsSuccess);

        var slot = result.Value;
        Assert.Equal(date,      slot.Date);
        Assert.Equal(startTime, slot.StartTime);
        Assert.Equal(duration,  slot.Duration);
        Assert.Equal(startTime.Add(duration), slot.EndTime);
    }

    [Fact]
    public void Create_NonPositiveDuration_ReturnsFailure()
    {
        var date      = new DateOnly(2025, 1, 10);
        var startTime = new TimeOnly(9, 0);
        var duration  = TimeSpan.Zero;

        var result = AppointmentSlot.Create(date, startTime, duration);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Appointment.DurationMustBePositive, result.Error);
    }

    [Fact]
    public void Create_TooLongDuration_ReturnsFailure()
    {
        var date      = new DateOnly(2025, 1, 10);
        var startTime = new TimeOnly(9, 0);
        var duration  = TimeSpan.FromHours(5); // > 4h
        
        var result = AppointmentSlot.Create(date, startTime, duration);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Appointment.DurationTooLong, result.Error);
    }
}