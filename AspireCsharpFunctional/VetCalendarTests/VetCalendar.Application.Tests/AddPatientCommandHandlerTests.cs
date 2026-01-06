using Moq;
using VetCalendar.Application.AddPatient;
using VetCalendar.Domain;
using VetCalendar.Domain.Clients;
using VetCalendar.Domain.Clients.Patients;

namespace VetCalendar.Application.Tests;

public class AddPatientCommandHandlerTests
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly AddPatientCommandHandler _sut;

    public AddPatientCommandHandlerTests()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _sut = new AddPatientCommandHandler(_clientRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsPatientAndReturnsSuccess()
    {
        var clientResult = Client.Create(
            firstName: "John",
            lastName: "Doe",
            email: "john.doe@example.com",
            phoneNumber: "+33601020304");

        var client = clientResult.Value;
        var clientId = client.Id;

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(client);

        var command = new AddPatientCommand(
            ClientId: clientId.Value,
            Name: "Misty",
            Species: "Cat",
            BirthDate: new DateOnly(2020, 5, 12));

        var result = await _sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        var patient = Assert.Single(client.Patients);
        Assert.Equal("Misty", patient.Name);
        Assert.Equal("Cat", patient.Species);
        Assert.Equal(new DateOnly(2020, 5, 12), patient.BirthDate);

        _clientRepositoryMock.Verify(r =>
                r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task Handle_ClientNotFound_ReturnsFailure()
    {
        var unknownClientId = new ClientId(Guid.NewGuid());

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(unknownClientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Client?)null);

        var command = new AddPatientCommand(
            ClientId: unknownClientId.Value,
            Name: "Misty",
            Species: "Cat",
            BirthDate: new DateOnly(2020, 5, 12));

        var result = await _sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Client.NotFound, result.Error);

        _clientRepositoryMock.Verify(
            r => r.GetByIdAsync(unknownClientId, It.IsAny<CancellationToken>()),
            Times.Once);

        _clientRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_InvalidPatientName_ReturnsFailureAndDoesNotAddPatient()
    {
        var clientResult = Client.Create(
            firstName: "John",
            lastName: "Doe",
            email: "john.doe@example.com",
            phoneNumber: "+33601020304");

        var client = clientResult.Value;
        var clientId = client.Id;

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(client);

        var command = new AddPatientCommand(
            ClientId: clientId.Value,
            Name: "",                 
            Species: "Cat",
            BirthDate: new DateOnly(2020, 5, 12));

        var result = await _sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Patient.NameIsRequired, result.Error);

        Assert.Empty(client.Patients);

        _clientRepositoryMock.Verify(
            r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()),
            Times.Once);

        _clientRepositoryMock.VerifyNoOtherCalls();
    }


    [Fact]
    public async Task Handle_ValidCommand_AddsPatient_And_RaisesPatientAddedEvent()
    {
        var clientResult = Client.Create(
            firstName: "John",
            lastName: "Doe",
            email: "john.doe@example.com",
            phoneNumber: "+33601020304");

        var client = clientResult.Value;
        client.ClearDomainEvents();
        var clientId = client.Id;

        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(client);

        var command = new AddPatientCommand(
            ClientId: clientId.Value,
            Name: "Misty",
            Species: "Cat",
            BirthDate: new DateOnly(2020, 5, 12));

        var result = await _sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        var patient = Assert.Single(client.Patients);
   

        var patientAddedEvent = Assert.Single(
            client.DomainEvents.OfType<PatientAddedDomainEvent>());

        Assert.Equal(client.Id, patientAddedEvent.ClientId);
        Assert.Equal(patient.Id, patientAddedEvent.PatientId);
    }

}