using CSharpFunctionalExtensions;
using VetCalendar.Application.Abstractions;
using VetCalendar.Domain;
using VetCalendar.Domain.Customers;
using VetCalendar.Domain.Patients;

namespace VetCalendar.Application.AddPatient;

public sealed class AddPatientCommandHandler : ICommandHandler<AddPatientCommand>
{
    private readonly IClientRepository _clientRepository;

    public AddPatientCommandHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result> Handle(AddPatientCommand command, CancellationToken cancellationToken)
    {
        return  await Maybe.From(async () => await _clientRepository.GetByIdAsync(new ClientId(command.ClientId), cancellationToken))
            .ToResult(DomainErrors.Client.NotFound)
            .Bind(client => client.AddPatient(command.Name, command.Species, command.BirthDate));
    }
}