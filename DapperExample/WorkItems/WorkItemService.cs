using Dapper;
using DapperExample.Database;
using DapperExample.Validation;
using DapperExample.WorkItems;
using FluentValidation;

namespace DapperExample;

public interface IWorkItemService
{
    public Task<Result<WorkItem, ValidationFailed>> Create(WorkItem workItem);
    public Task<WorkItem?> GetById(int id);
    public Task<IEnumerable<WorkItem>> GetAll();
    public Task<Result<WorkItem?, ValidationFailed>> Update(WorkItem workItem);
    public Task<bool> DeleteById(int id);
}

public class WorkItemService : IWorkItemService
{
    private readonly IValidator<WorkItem> _validator;
    private readonly IDbConnectionFactory _connectionFactory;

    public WorkItemService(IValidator<WorkItem> validator, IDbConnectionFactory connectionFactory)
    {
        _validator = validator;
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<WorkItem, ValidationFailed>> Create(WorkItem workItem)
    {
        var validationResult = await _validator.ValidateAsync(workItem);
        if(!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync(
            """
            INSERT INTO Tasks 
                (TaskName, Description, StartDate, DueDate, 
                 ProjectId, StatusId, Estimate, PriorityId, 
                 CreatedDate, ModifiedDate)
            VALUES 
                (@TaskName, @Description, @StartDate, @DueDate, 
                 @ProjectId, @StatusId, @Estimate, @PriorityId, 
                 datetime('now'), datetime('now')),
            """,
            workItem
        );

        return workItem;
    }

    public async Task<WorkItem?> GetById(int id)
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        var workItem = await dbConnection.QuerySingleOrDefaultAsync<WorkItem>(
            "SELECT * FROM Tasks WHERE ID = @Id;",
            new { id }
        );

        return workItem;
    }

    public async Task<IEnumerable<WorkItem>> GetAll()
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        return await dbConnection.QueryAsync<WorkItem>(
            "SELECT * FROM Tasks;"
        );
    }

    public async Task<Result<WorkItem?, ValidationFailed>> Update(WorkItem workItem)
    {
        var validationResult = await _validator.ValidateAsync(workItem);
        if(!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        var existingWorkItem = await GetById(workItem.Id);
        if(existingWorkItem is null)
        {
            return default(WorkItem?);
        }

        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync(
            """
            UPDATE Tasks
            SET TaskName = @TaskName,
                Description = @Description,
                StartDate = @StartDate,
                DueDate = @DueDate,
                ProjectId = @ProjectId,
                StatudId = @StatusId,
                Estimate = @Estimate,
                PriorityId = @PriorityId
            WHERE ID = @Id
            """,
            workItem
        );

        return workItem;
    }

    public async Task<bool> DeleteById(int id)
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        var deleted = await dbConnection.ExecuteAsync(
            "DELETE FROM Tasks WHERE ID = @Id;",
            new { id }
        );
        return deleted > 0;
    }
}
