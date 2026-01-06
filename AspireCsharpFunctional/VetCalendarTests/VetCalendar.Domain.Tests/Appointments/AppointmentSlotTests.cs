using VetCalendar.Domain.Appointments;

namespace VetCalendar.Domain.Tests.Appointments;

public class AppointmentSlotTests
{
    [Fact]
    public void Create_ValidData_ReturnsSuccess()
    {
        var date      = new DateOnly(2025, 1, 10);
        var startTime = new TimeOnly(9, 0);

        var result = AppointmentSlot.Create(date, startTime);

        Assert.True(result.IsSuccess);

        var slot = result.Value;
        Assert.Equal(date,      slot.Date);
        Assert.Equal(startTime, slot.StartTime);
        Assert.Equal(startTime.Add(TimeSpan.FromMinutes(30)), slot.EndTime);
    }

    [Fact]
    public void Create_InvalidMinute_ReturnsFailure()
    {
        var date = new DateOnly(2025, 1, 10);
        var start = new TimeOnly(9, 10);

        var result = AppointmentSlot.Create(date, start);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Appointment.InvalidStartMinute, result.Error);
    }

    [Theory]
    [InlineData(9, 0, 1, 0)]
    [InlineData(9, 0, 0, 1)]
    [InlineData(9, 0, 50, 0)]
    public void Create_InvalidWithSecond_ReturnsFailure(int hour, int minute, int second, int milliSecond)
    {
        var date = new DateOnly(2025, 1, 10);
        var start = new TimeOnly(hour, minute, second, milliSecond);

        var result = AppointmentSlot.Create(date, start);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Appointment.InvalidStartTime, result.Error);
    }
}