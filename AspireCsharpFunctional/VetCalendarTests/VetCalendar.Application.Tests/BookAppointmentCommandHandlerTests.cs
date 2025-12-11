using Moq;
using VetCalendar.Application.BookAppointment;
using VetCalendar.Domain;
using VetCalendar.Domain.Appointments;
using VetCalendar.Domain.Clients;
using VetCalendar.Domain.Clients.Patients;

namespace VetCalendar.Application.Tests;

public class BookAppointmentCommandHandlerTests
{
    private readonly Mock<IClientRepository> _clientRepositoryMock = new();
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock = new();
    private readonly BookAppointmentCommandHandler _sut;

    public BookAppointmentCommandHandlerTests()
    {
        _sut = new BookAppointmentCommandHandler(
            _clientRepositoryMock.Object,
            _appointmentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_BooksAppointment_AndPersistsIt()
    {
        var clientResult = Client.Create(
            firstName: "John",
            lastName: "Doe",
            email: "john.doe@example.com",
            phoneNumber: "+33601020304");

        var client = clientResult.Value;

        var patientResult = client.AddPatient(
            name: "Misty",
            species: "Cat",
            birthDate: new DateOnly(2020, 5, 1));

        Assert.True(patientResult.IsSuccess);
        var patient = patientResult.Value;

        client.ClearDomainEvents(); 

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(client.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(client);

        var date      = new DateOnly(2025, 1, 10);
        var startTime = new TimeOnly(9, 0);
        var reason    = "Consultation";

        var command = new BookAppointmentCommand(
            ClientId:  client.Id.Value,
            PatientId: patient.Id.Value,
            Date:      date,
            StartTime: startTime,
            Reason:    reason);

        var result = await _sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        _appointmentRepositoryMock.Verify(
            r => r.AddAsync(
                It.Is<Appointment>(a =>
                    a.ClientId  == client.Id &&
                    a.PatientId == patient.Id &&
                    a.Slot.Date == date &&
                    a.Slot.StartTime == startTime &&
                    a.Reason == reason &&
                    a.Status == AppointmentStatus.Booked),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ClientNotFound_ReturnsFailure_AndDoesNotPersistAppointment()
    {
        // Arrange
        var unknownClientId = new ClientId(Guid.NewGuid());
        var somePatientId   = new PatientId(Guid.NewGuid());

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(unknownClientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Client?)null);

        var command = new BookAppointmentCommand(
            ClientId:  unknownClientId.Value,
            PatientId: somePatientId.Value,
            Date:      new DateOnly(2025, 1, 10),
            StartTime: new TimeOnly(9, 0),
            Reason:    "Whatever");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Client.NotFound, result.Error);

        _appointmentRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

}