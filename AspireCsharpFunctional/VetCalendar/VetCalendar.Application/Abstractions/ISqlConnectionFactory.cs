using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace VetCalendar.Application.Abstractions;

public interface ISqlConnectionFactory
{
    IDbConnection CreateOpenConnection(CancellationToken cancellationToken = default);
}
public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        this._connectionString = connectionString;
    }

    public IDbConnection CreateOpenConnection(CancellationToken cancellationToken = default) 
    {
        var connection = new SqlConnection(_connectionString);
        connection.OpenAsync(cancellationToken);
        return connection;
    }
}