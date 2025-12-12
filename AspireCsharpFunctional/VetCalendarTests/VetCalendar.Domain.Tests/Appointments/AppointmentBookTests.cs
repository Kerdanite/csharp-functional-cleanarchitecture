using VetCalendar.Domain.Appointments;
using VetCalendar.Domain.Clients;
using VetCalendar.Domain.Clients.Patients;

namespace VetCalendar.Domain.Tests.Appointments;

public class AppointmentBookTests
{
    [Fact]
    public void Book_ValidData_ReturnsBookedAppointment()
    {
        var clientId = ClientId.New();
        var patientId = PatientId.New();

        var date = new DateOnly(2025, 1, 10);
        var startTime = new TimeOnly(9, 0);

        var slotResult = AppointmentSlot.Create(date, startTime);
        Assert.True(slotResult.IsSuccess); 
        var slot = slotResult.Value;

        var reason = "Consultation générale";

        var result = Appointment.Book(
            clientId,
            patientId,
            slot,
            reason);

        Assert.True(result.IsSuccess);

        var appointment = result.Value;

        Assert.Equal(clientId, appointment.ClientId);
        Assert.Equal(patientId, appointment.PatientId);
        Assert.Equal(slot, appointment.Slot);
        Assert.Equal(reason, appointment.Reason);
        Assert.Equal(AppointmentStatus.Booked, appointment.Status);
    }


    [Fact]
    public void Book_ReasonTooLong_ReturnsFailure()
    {
        var clientId  = ClientId.New();
        var patientId = PatientId.New();

        var slotResult = AppointmentSlot.Create(
            new DateOnly(2025, 1, 10),
            new TimeOnly(9, 0));

        Assert.True(slotResult.IsSuccess);
        var slot = slotResult.Value;

        var longReason = new string('a', 501); 

        // Act
        var result = Appointment.Book(
            clientId,
            patientId,
            slot,
            longReason);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Appointment.ReasonTooLong, result.Error);
    }

    [Fact]
    public void Book_NullReason_IsAcceptedAndNormalizedToEmptyString()
    {
        var clientId  = ClientId.New();
        var patientId = PatientId.New();

        var slotResult = AppointmentSlot.Create(
            new DateOnly(2025, 1, 10),
            new TimeOnly(9, 0));

        Assert.True(slotResult.IsSuccess);
        var slot = slotResult.Value;

        string? reason = null;

        var result = Appointment.Book(
            clientId,
            patientId,
            slot,
            reason);

        Assert.True(result.IsSuccess);

        var appointment = result.Value;
        Assert.Equal(string.Empty, appointment.Reason);
    }

    [Fact]
    public void Book_ValidData_RaisesAppointmentBookedDomainEvent()
    {
        var clientId  = ClientId.New();
        var patientId = PatientId.New();

        var slotResult = AppointmentSlot.Create(
            new DateOnly(2025, 1, 10),
            new TimeOnly(9, 0));

        Assert.True(slotResult.IsSuccess);
        var slot = slotResult.Value;

        var reason = "Consultation générale";

        var result = Appointment.Book(
            clientId,
            patientId,
            slot,
            reason);

        Assert.True(result.IsSuccess);

        var appointment = result.Value;

        var evt = Assert.Single(
            appointment.DomainEvents.OfType<AppointmentBookedDomainEvent>());

        Assert.Equal(appointment.Id, evt.AppointmentId);
    }
}