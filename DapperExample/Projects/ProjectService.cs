using DapperExample.Database;
using DapperExample.Projects;
using DapperExample.Validation;
using FluentValidation;

namespace DapperExample;

public interface IProjectService
{
    public Task<Result<Project, ValidationFailed>> Create(Project project);
    public Task<Project?> GetById(int id);
    public Task<IEnumerable<Project>> GetAll();
    public Task<Result<Project?, ValidationFailed>> Update(Project project);
    public Task<bool> DeleteById(int id);
}

public class ProjectServic : IProjectService
{
    private readonly IValidator<Project> _validator;
    private readonly IDbConnectionFactory _connectionFactory;

    public ProjectServic(IValidator<Project> validator, IDbConnectionFactory connectionFactory)
    {
        _validator = validator;
        _connectionFactory = connectionFactory;
    }

    public Task<Result<Project, ValidationFailed>> Create(Project project)
    {
        throw new NotImplementedException();
    }

    public Task<Project?> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Project>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Result<Project?, ValidationFailed>> Update(Project project)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteById(int id)
    {
        throw new NotImplementedException();
    }
}

