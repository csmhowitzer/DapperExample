using DapperExample.Validation;
using DapperExample.Roles;
using FluentValidation;
using DapperExample.Database;
using Dapper;

namespace DapperExample;

public interface IRoleService
{
    public Task<Result<Role, ValidationFailed>> Create(Role role);
    public Task<Role?> GetById(int id);
    public Task<IEnumerable<Role>> GetAll();
    public Task<Result<Role?, ValidationFailed>> Update(Role role);
    public Task<bool> DeleteById(int id);
}

public class RoleService : IRoleService
{
    private readonly IValidator<Role> _validator;
    private readonly IDbConnectionFactory _connectionFactory;

    public RoleService(IValidator<Role> validator, IDbConnectionFactory connectionFactory)
    {
        _validator = validator;
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<Role, ValidationFailed>> Create(Role role)
    {
        var validationResult = await _validator.ValidateAsync(role);
        if(!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync(
            """
            INSERT INTO Roles (Name, Desc)
            VALUES (@Name, @Desc);
            """,
            role
        );

        return role;
    }

    public async Task<Role?> GetById(int id)
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        var role = await dbConnection.QuerySingleOrDefaultAsync<Role>(
            "SELECT * FROM Roles WHERE ID = @Id LIMIT 1;",
            new { id }
        );
        return role;
    }

    public async Task<IEnumerable<Role>> GetAll()
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        return await dbConnection.QueryAsync<Role>(
            "SELECT * FROM Roles;"
        );
    }

    public async Task<Result<Role?, ValidationFailed>> Update(Role role)
    {
        var validationResult = await _validator.ValidateAsync(role);
        if(!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        var existingRole = await GetById(role.Id);
        if(existingRole is null)
        {
            return default(Role?);
        }

        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync(
            """
            UPDATE Roles
            SET Name = @Name,
                Desc = @Desc
            WHERE ID = @Id
            """,
            role
        );

        return role;
    }

    public async Task<bool> DeleteById(int id)
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        var deleted = await dbConnection.ExecuteAsync(
            "DELETE FROM Roles WHERE ID = @Id;",
            new { id }
        );
        return deleted > 0;
    }

}

