using CSharpFunctionalExtensions;
using Dapper;
using VetCalendar.Application.Abstractions;
using VetCalendar.Domain;

namespace VetCalendar.Application.GetClientPatients;

public sealed class GetClientPatientsQueryHandler
    : IQueryHandler<GetClientPatientsQuery, IReadOnlyList<PatientDto>>
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetClientPatientsQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<IReadOnlyList<PatientDto>>> Handle(
        GetClientPatientsQuery query,
        CancellationToken ct = default)
    {
        const string sql = @"
SELECT CAST(1 AS bit)
FROM Clients
WHERE Id = @ClientId;

SELECT
    p.Id,
    p.Name,
    p.Species,
    p.BirthDate
FROM Patients p
WHERE p.ClientId = @ClientId
ORDER BY p.Name;";

        using var connection = _connectionFactory.CreateOpenConnection(ct);

        using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, new { query.ClientId }, cancellationToken: ct));

        var clientExists = await multi.ReadFirstOrDefaultAsync<bool>();
        if (!clientExists)
            return Result.Failure<IReadOnlyList<PatientDto>>(DomainErrors.Client.NotFound);

        var patients = (await multi.ReadAsync<PatientRow>()).ToList();

        return Result.Success<IReadOnlyList<PatientDto>>(patients.Select(s => new PatientDto(s.Id, s.Name, s.Species, DateOnly.FromDateTime(s.BirthDate))).ToList());
    }

    internal sealed record PatientRow(
        Guid Id,
        string Name,
        string Species,
        DateTime BirthDate);
}