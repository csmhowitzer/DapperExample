using DapperExample.Employees;
using DapperExample.Validation;
using Dapper;
using FluentValidation;
using DapperExample.Database;

namespace DapperExample;

public interface IEmployeeService 
{
    public Task<Result<Employee, ValidationFailed>> Create(Employee dto);
    public Task<Employee?> GetById(int id);
    public Task<IEnumerable<Employee>> GetAll();
    public Task<Result<Employee?, ValidationFailed>> Update(Employee dto);
    public Task<bool> DeleteById(int id);
}

public class EmployeeService : IEmployeeService
{
    private readonly IValidator<Employee> _validator;
    private readonly IDbConnectionFactory _connectionFactory;

    public EmployeeService(IValidator<Employee> validator, IDbConnectionFactory connectionFactory)
    {
        _validator = validator;
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<Employee, ValidationFailed>> Create(Employee employee)
    {
        var validationResult = await _validator.ValidateAsync(employee);
        if (!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync(
                """
                INSERT INTO Employees (FName, LName, Email, CreatedDate, ModifiedDate)
                VALUES (@FName, @LName, @Email, datetime('now'), datetime('now'));

                INSERT INTO EmployeeRoles (EmployeeId, RoleId, CreatedDate)
                SELECT
                    e.ID,
                    r.ID,
                    datetime('now')
                FROM 
                (
                    SELECT ID FROM Employees
                    WHERE FName = @FName
                    AND LName = @LName
                    AND Email = @Email
                ) AS e,
                (
                    SELECT ID FROM Roles
                    WHERE Name = @RoleName
                ) AS r
                WHERE EXISTS (SELECT 1 FROM Roles WHERE Name = @RoleName);
                """,
                employee
        );
        dbConnection.Dispose();

        return employee;
    }

    public async Task<Employee?> GetById(int id)
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        var employee = await dbConnection.QuerySingleOrDefaultAsync<Employee>(
            $"""
            SELECT 
                e.ID AS {nameof(Employee.Id)},
                e.FName AS {nameof(Employee.FName)},
                e.LName AS {nameof(Employee.LName)},
                e.Email AS {nameof(Employee.Email)},
                r.Name AS {nameof(Employee.RoleName)}
            FROM Employees e
            INNER JOIN EmployeeRoles er ON e.ID = er.EmployeeId
            INNER JOIN Roles r ON er.RoleId = r.ID
            WHERE e.ID = @id LIMIT 1;
            """,
            new { id }
        );
        return employee;
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        return await dbConnection.QueryAsync<Employee>(
            $"""
            SELECT 
                e.ID AS {nameof(Employee.Id)},
                e.FName AS {nameof(Employee.FName)},
                e.LName AS {nameof(Employee.LName)},
                e.Email AS {nameof(Employee.Email)},
                r.Name AS {nameof(Employee.RoleName)}
            FROM Employees e
            INNER JOIN EmployeeRoles er ON e.ID = er.EmployeeId
            INNER JOIN Roles r ON er.RoleId = r.ID
            """
        );
    }

    public async Task<Result<Employee?, ValidationFailed>> Update(Employee employee)
    {
        var validationResult = await _validator.ValidateAsync(employee);
        if (!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        var existingEmployee = await GetById(employee.Id);
        if (existingEmployee is null)
        {
            return default(Employee?);
        }

        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync(
            """
            UPDATE Employees 
            SET FName = @FName,
                LName = @LName,
                Email = @Email,
                ModifiedDate = datetime('now')
            WHERE ID = @Id
            """,
            employee
        );

        return employee;
    }

    public async Task<bool> DeleteById(int id)
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        var result = await dbConnection.ExecuteAsync(
            "DELETE FROM Employees WHERE ID = @id;",
            new { id }
        );
        return result > 0;
    }
}
