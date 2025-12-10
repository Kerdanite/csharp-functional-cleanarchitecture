using CSharpFunctionalExtensions;
using VetCalendar.Application.Abstractions;
using VetCalendar.Domain;
using VetCalendar.Domain.Customers;

namespace VetCalendar.Application.CreateClient;

public sealed class CreateClientCommandHandler : ICommandHandler<CreateClientCommand>
{
    private readonly IClientRepository _clientRepository;

    public CreateClientCommandHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result> Handle(CreateClientCommand command, CancellationToken cancellationToken)
    {
        return await Result.Success()
            .Ensure(async() => await VerifyEmailNotAlreadyExists(command, cancellationToken))
            .Ensure(async() => await VerifyPhoneNumberNotAlreadyExists(command, cancellationToken))
            .Bind(() =>
                Client.Create(
                    command.FirstName,
                    command.LastName,
                    command.Email,
                    command.PhoneNumber))
            .Tap(async client =>
                await _clientRepository.AddAsync(client, cancellationToken));
    }

    private async Task<Result> VerifyEmailNotAlreadyExists(CreateClientCommand command, CancellationToken ct)
    {
        var exist =  await _clientRepository.EmailExistsAsync(command.Email, ct);
        return exist ? Result.Failure(DomainErrors.Client.EmailAlreadyInUse) : Result.Success();
    }
    private async Task<Result> VerifyPhoneNumberNotAlreadyExists(CreateClientCommand command, CancellationToken ct)
    {
        var exist =  await _clientRepository.PhoneNumberExistsAsync(command.PhoneNumber, ct);
        return exist ? Result.Failure(DomainErrors.Client.PhoneNumberAlreadyInUse) : Result.Success();
    }
}