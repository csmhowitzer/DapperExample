using System.Data;
using Microsoft.Data.Sqlite;

namespace DapperExample.Database;

public class SqliteDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteDbConnectionFactory(string connectionString)
    {
        // accounting for MacOs $HOME paths
        _connectionString = connectionString.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(token);
        return connection;
    }
}

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}
