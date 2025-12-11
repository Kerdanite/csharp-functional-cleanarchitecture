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
}