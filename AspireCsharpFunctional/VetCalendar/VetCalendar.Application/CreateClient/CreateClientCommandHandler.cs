using CSharpFunctionalExtensions;
using VetCalendar.Domain.Customers;

namespace VetCalendar.Application.CreateClient;

public sealed class CreateClientCommandHandler
{
    private readonly IClientRepository _clientRepository;

    public CreateClientCommandHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result> Handle(CreateClientCommand command, CancellationToken ct = default)
    {
        var clientResult = Client.Create(
            command.FirstName,
            command.LastName,
            command.Email,
            command.PhoneNumber);

        if (clientResult.IsFailure)
            return Result.Failure(clientResult.Error);

        var client = clientResult.Value;

        await _clientRepository.AddAsync(client, ct);

        return Result.Success();
    }
}