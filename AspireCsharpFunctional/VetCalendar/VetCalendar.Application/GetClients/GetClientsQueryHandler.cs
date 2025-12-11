using CSharpFunctionalExtensions;
using Dapper;
using VetCalendar.Application.Abstractions;

namespace VetCalendar.Application.GetClients;

public class GetClientsQueryHandler : IQueryHandler<GetClientsQuery, IReadOnlyList<ClientDto>>
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetClientsQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<IReadOnlyList<ClientDto>>> Handle(GetClientsQuery query, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT
            c.Id,
            c.FirstName,
            c.LastName,
            c.Email,
            c.PhoneNumber,
            COUNT(p.Id) AS PatientsCount
        FROM Clients c
        LEFT JOIN Patients p ON p.ClientId = c.Id
        GROUP BY
            c.Id,
            c.FirstName,
            c.LastName,
            c.Email,
            c.PhoneNumber";

        using var connection = _connectionFactory.CreateOpenConnection(cancellationToken);

        var result =  await connection.QueryAsync<ClientDto>(sql, cancellationToken);

        return Result.Success<IReadOnlyList<ClientDto>>(result.ToList());
    }
}