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
                Id,
                FirstName,
                LastName,
                Email,
                PhoneNumber
            FROM Clients";

        using var connection = _connectionFactory.CreateOpenConnection(cancellationToken);

        var result =  await connection.QueryAsync<ClientDto>(sql, cancellationToken);

        return Result.Success<IReadOnlyList<ClientDto>>(result.ToList());
    }
}