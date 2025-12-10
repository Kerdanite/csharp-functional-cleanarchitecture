using Moq;
using VetCalendar.Application.CreateClient;
using VetCalendar.Domain;
using VetCalendar.Domain.Customers;

namespace VetCalendar.Application.Tests;

public class CreateClientCommandHandlerTests
{
    private readonly Mock<IClientRepository> _repoMock;
    private readonly CreateClientCommandHandler _sut;

    public CreateClientCommandHandlerTests()
    {
        _repoMock = new Mock<IClientRepository>();
        _sut = new CreateClientCommandHandler(_repoMock.Object);
    }


    [Fact]
    public async Task Handle_ValidCommand_SavesClientInRepository()
    {
        var command = new CreateClientCommandBuilder().Build();

        var result = await _sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        _repoMock.Verify(r => r.AddAsync(
                It.Is<Client>(c =>
                    c.FirstName == command.FirstName &&
                    c.LastName == command.LastName &&
                    c.Email == command.Email &&
                    c.PhoneNumber == command.PhoneNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_InvalidFirstName_ReturnsFailureAndDoesNotSaveClient(string invalidFirstName)
    {
        var command = new CreateClientCommandBuilder()
            .WithFirstName(invalidFirstName)
            .Build();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Client.FirstNameIsRequired, result.Error);

        _repoMock.Verify(r => r.AddAsync(
                It.IsAny<Client>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_InvalidLastName_ReturnsFailureAndDoesNotSaveClient(string invalidLastName)
    {
        var command = new CreateClientCommandBuilder()
            .WithLastName(invalidLastName)
            .Build();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Client.LastNameIsRequired, result.Error);

        _repoMock.Verify(r => r.AddAsync(
                It.IsAny<Client>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_InvalidEmptyEmail_ReturnsFailureAndDoesNotSaveClient(string invalidEmail)
    {
        var command = new CreateClientCommandBuilder()
            .WithEmail(invalidEmail)
            .Build();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Client.EmailIsRequired, result.Error);

        _repoMock.Verify(r => r.AddAsync(
                It.IsAny<Client>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}

public class CreateClientCommandBuilder
{
    private string _email = "john.doe@example.com";
    private string _firstName = "John";
    private string _lastName = "Doe";
    private string _phoneNumber = "+33601020304";

    public CreateClientCommandBuilder WithFirstName(string value)
    {
        _firstName = value;
        return this;
    }

    public CreateClientCommandBuilder WithLastName(string value)
    {
        _lastName = value;
        return this;
    }

    public CreateClientCommandBuilder WithEmail(string value)
    {
        _email = value;
        return this;
    }

    public CreateClientCommandBuilder WithPhoneNumber(string value)
    {
        _phoneNumber = value;
        return this;
    }

    public CreateClientCommand Build()
    {
        return new CreateClientCommand(_firstName, _lastName, _email, _phoneNumber);
    }
}

