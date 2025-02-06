using FluentValidation;
using DapperExample.Employees;
using DapperExample.Database;
using DapperExample.Roles;
using DapperExample.WorkItems;
using DapperExample.Projects;

namespace DapperExample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IDbConnectionFactory>(_ => 
                new SqliteDbConnectionFactory(
                    builder.Configuration["DbConnectionString"]!
                )
        );

        builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
        builder.Services.AddSingleton<IRoleService, RoleService>();
        builder.Services.AddSingleton<IWorkItemService, WorkItemService>();
        builder.Services.AddSingleton<IProjectService, ProjectService>();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

        var app = builder.Build();

        if(app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapEmployeeEndpoints();
        app.MapRoleEndpoints();
        app.MapWorkItemEndpoints();
        app.MapProjectEndpoints();

        app.Run();
    }
}
