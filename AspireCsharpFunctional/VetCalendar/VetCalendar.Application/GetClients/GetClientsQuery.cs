using VetCalendar.Application.Abstractions;

namespace VetCalendar.Application.GetClients;

public sealed record GetClientsQuery:  IQuery<IReadOnlyList<ClientDto>>;