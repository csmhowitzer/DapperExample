using DapperExample.Database;
using DapperExample.Employees;
using DapperExample.Projects;
using DapperExample.Roles;
using DapperExample.WorkItems;
using FluentValidation;

namespace DapperExample;

public class Program
{
    public static void Main(string[] args)
    {
        var MyAllowedOrigin = "_myAllowedOrigin";

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                name: MyAllowedOrigin,
                policy =>
                {
                    policy.WithOrigins("http://localhost:5173"); // vue proj
                }
            );
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IDbConnectionFactory>(_ => new SqliteDbConnectionFactory(
            builder.Configuration["DbConnectionString"]!
        ));

        builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
        builder.Services.AddSingleton<IRoleService, RoleService>();
        builder.Services.AddSingleton<IWorkItemService, WorkItemService>();
        builder.Services.AddSingleton<IProjectService, ProjectService>();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(MyAllowedOrigin);

        app.MapEmployeeEndpoints();
        app.MapRoleEndpoints();
        app.MapWorkItemEndpoints();
        app.MapProjectEndpoints();

        app.Run();
    }
}
