using Dapper;
using DapperExample.Database;
using DapperExample.Projects;
using DapperExample.Validation;
using DapperExample.WorkItems;
using FluentValidation;

namespace DapperExample;

public interface IProjectService
{
    public Task<Result<Project, ValidationFailed>> Create(Project project);
    public Task<Project?> GetById(int id);
    public Task<ProjectTask?> GetNestedById(int id);
    public Task<IEnumerable<Project>> GetAll();
    public Task<Result<Project?, ValidationFailed>> Update(Project project);
    public Task<bool> DeleteById(int id);
}

public class ProjectService : IProjectService
{
    private readonly IValidator<Project> _validator;
    private readonly IDbConnectionFactory _connectionFactory;

    public ProjectService(IValidator<Project> validator, IDbConnectionFactory connectionFactory)
    {
        _validator = validator;
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<Project, ValidationFailed>> Create(Project project)
    {
        var validationResult = await _validator.ValidateAsync(project);
        if(!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync(
            """
            INSERT INTO Projects (Name, Description, StartDate, EndDate, OwnerId) 
            VALUES (@Name, @Description, @StartDate, @EndDate, @OwnerId);
            """,
            project
        );

        return project;
    }

    public async Task<Project?> GetById(int id)
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        var project = await dbConnection.QuerySingleOrDefaultAsync<Project>(
            "SELECT * FROM Projects WHERE ID = @Id;",
            new { id }
        );
        return project;
    }

    public async Task<IEnumerable<Project>> GetAll()
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        return await dbConnection.QueryAsync<Project>(
            "SELECT * FROM Projects;"
        );
    }

    public async Task<ProjectTask?> GetNestedById(int id)
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        var project = await dbConnection.QueryAsync<ProjectTask?, WorkItem, ProjectTask?>(
            """
            SELECT
                p.Id,
                p.Name,
                p.Description,
                t.TaskName,
                t.Description,
                t.DueDate
            FROM Projects p
            INNER JOIN Tasks t ON p.ID = t.ProjectId
            WHERE p.ID = @Id
            GROUP BY p.Id,
                     p.Name,
                     p.Description,
                     t.TaskName,
                     t.Description,
                     t.DueDate;
            """,
            (proj, workItem) =>
            {
                if(proj is null)
                {
                    return default(ProjectTask);
                }
                if(proj.WorkItems is null)
                {
                    proj.WorkItems = new List<WorkItem>();
                }

                proj.WorkItems.Add(workItem);
                return proj;

            },
            new { Id = id },
            splitOn: "TaskName"
        );
        return (ProjectTask?)project.FirstOrDefault();
    }

    public async Task<Result<Project?, ValidationFailed>> Update(Project project)
    {
        var validationResult = await _validator.ValidateAsync(project);
        if(!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        var existingProj = await GetById(project.Id);
        if(existingProj is null)
        {
            return default(Project?);
        }

        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync(
            """
            UPDATE Projects
            SET Name = @Name,
                Description = @Description,
                StartDate = @StartDate,
                EndDate = @EndDate,
                OwnerId = @OwnerId
            WHERE ID = @Id;
            """,
            project
        );

        return project;
    }

    public async Task<bool> DeleteById(int id)
    {
        using var dbConnection = await _connectionFactory.CreateConnectionAsync();
        var deleted = await dbConnection.ExecuteAsync(
            "DELETE FROM Projects WHERE ID = @Id;",
            new { id }
        );
        return deleted > 0;
    }
}

